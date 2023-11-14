using ArivalBank._2fa.Application.Interfaces;
using ArivalBank._2fa.Application.Models;
using ArivalBank._2fa.Domain.Configuration;
using ArivalBank._2fa.Domain.Entities;
using ArivalBank._2fa.Application.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System;
using System.Text;


namespace ArivalBank._2fa.Application.Authorization
{
    public class AuthorizationCodeService : IAuthorizationCodeService
    {
        private readonly ILogger<AuthorizationCodeService> _logger;
        private readonly AppConfiguration _appConfiguration;
        private readonly IRepository<AuthorizationCode> _repository;
        private readonly ISmsGatewayMessaging _smsGatewayMessaging;
        private readonly IDistributedCache _cache;
        private DistributedCacheEntryOptions _redisDistributedCacheOptions;

        public AuthorizationCodeService(ILogger<AuthorizationCodeService> logger, 
            IOptions<AppConfiguration> appConfiguration, 
            IRepository<AuthorizationCode> repository,
            ISmsGatewayMessaging smsGatewayMessaging,
            IDistributedCache cache) 
        {
            _logger = logger;
            _appConfiguration = appConfiguration.Value;
            _repository = repository;
            _smsGatewayMessaging = smsGatewayMessaging;
            _cache = cache;
            _redisDistributedCacheOptions = new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(_appConfiguration.CodeDurationMinutes));
        }


        public async Task<AuthorizationCode> GenerateAuthorizationCode(AuthorizationCodeRequestModel authenticationCodeRequestModel)
        {
            var authCode = new AuthorizationCode();
            authCode.Code = GenerateCode();
            authCode.Phone = authenticationCodeRequestModel.PhoneNumber;
            authCode.ExpirationTime = DateTime.Now.AddMinutes(_appConfiguration.CodeDurationMinutes);

            var activeCodes = await GetTotalActiveCodes(authenticationCodeRequestModel);

            if (activeCodes < _appConfiguration.MaxCodesByPhone)
            {
                await _repository.Save(authCode);
                await _cache.SetAsync(GenerateCacheKey(authCode), authCode, _redisDistributedCacheOptions);

                if (_appConfiguration.IsSmsActive) 
                {
                    _smsGatewayMessaging.DeliverMessage(new SmsMessageModel());
                }
            }
            else 
            {
                throw new Exception($"GENERATE-AUTHORIZATION-CODE-ERROR:You can't generate more than 5 authentication codes {_appConfiguration.MaxCodesByPhone} codes, Please contact the administrator to increase the amount of generated codes");
            }

            return authCode;
        }


        public string GenerateCode()
        {
            var baseCode = Guid.NewGuid().ToString().Replace("-", string.Empty);
            var newCode = new StringBuilder();

            for (var i = 0; i < _appConfiguration.CodeLength; i++)
            {
                newCode.Append(baseCode[new Random().Next(baseCode.Length)].ToString());
            }

            return newCode.ToString().ToUpper();
        }

        
        public Task<AuthorizationCode?> ValidateAuthorizationCode(VerifyCodeRequestModel verifyCodeRequestModel)
        {
            var cachedAuthcode = GetAuthorizationCodeFromCache(verifyCodeRequestModel);
            var authCode = new AuthorizationCode();

            if (cachedAuthcode.IsCached)
            {
                authCode = cachedAuthcode.AuthorizationCode;
                _logger.LogInformation("Code found in cache.");
            }
            else 
            {
                authCode = _repository.ListAll()
                               .Where(w =>
                                       w.Code.Equals(verifyCodeRequestModel.Code.Trim().ToUpper()) &&
                                       w.Phone.Equals(verifyCodeRequestModel.PhoneNumber) &&
                                       IsCodeExpired(w.ExpirationTime))
                               .OrderByDescending(w => w.ExpirationTime)
                               .FirstOrDefault();
            }

            return Task.FromResult(authCode);
        }


        private CachedAuthorizationCodeModel GetAuthorizationCodeFromCache(VerifyCodeRequestModel verifyCodeRequestModel)
        {
            var cachedAuthCode = new CachedAuthorizationCodeModel();

            try
            {
                cachedAuthCode.IsCached = _cache.TryGetValue(GenerateCacheKey(verifyCodeRequestModel), out AuthorizationCode? code);
                cachedAuthCode.AuthorizationCode = code;
                return cachedAuthCode;
            }
            catch
            {
                _logger.LogInformation("Code Server is not working.");
                return cachedAuthCode;
            }
        }


        public Task<int> GetTotalActiveCodes(AuthorizationCodeRequestModel authenticationCodeRequestModel) 
        {
            var totalCodes = _repository.ListAll()
               .Where(w =>
                       w.Phone.Equals(authenticationCodeRequestModel.PhoneNumber) &&
                       IsCodeExpired(w.ExpirationTime)
               ).Count();
               
            return Task.FromResult(totalCodes);
        }
  

        public bool IsCodeExpired(DateTime expirationTime)
        {
            var lifeTimeMinutes = (int)(expirationTime - DateTime.Now).TotalMinutes;

            if (lifeTimeMinutes > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        private string GenerateCacheKey(AuthorizationCode authCode) 
        {
            return $"AuthCodesCache:{authCode.Code.Trim().ToUpper()}-{authCode.Phone}"; 
        }


        private string GenerateCacheKey(VerifyCodeRequestModel authCode)
        {
            return $"AuthCodesCache:{authCode.Code.Trim().ToUpper()}-{authCode.PhoneNumber}";
        }

    }
}