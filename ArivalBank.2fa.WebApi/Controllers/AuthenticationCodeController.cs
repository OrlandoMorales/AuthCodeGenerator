using ArivalBank._2fa.Application.Authorization;
using ArivalBank._2fa.Application.Interfaces;
using ArivalBank._2fa.Application.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ArivalBank._2fa.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationCodeController : ControllerBase
    {
        private readonly ILogger<AuthenticationCodeController> _logger;

        private readonly IAuthorizationCodeService _authorizationCodeService;

        public AuthenticationCodeController(ILogger<AuthenticationCodeController> logger, IAuthorizationCodeService authorizationCodeService)
        {
            _logger = logger;
            _authorizationCodeService = authorizationCodeService;
        }


        [Route("GenerateCode")]
        [HttpPost]
        public async Task<IActionResult> GenerateCode(AuthorizationCodeRequestModel authenticationCodeRequestModel)
        {
            try 
            {
                var authCode = await _authorizationCodeService.GenerateAuthorizationCode(authenticationCodeRequestModel);

                _logger.LogInformation($"New Code Was Generated:: {authCode.Code} to Phone Number:: {authCode.Phone}");
                return Ok("Code Successfully Sent...");
            }
            catch (Exception ex) 
            {
                _logger.LogError($"GENERATE-CODE-ERROR: {ex.Message}");
                return StatusCode(500,ex.Message);
            }
        }


        [Route("VerifyCode")]
        [HttpPost]
        public async Task<IActionResult> VerifyCode(VerifyCodeRequestModel verifyCodeRequestModel)
        {
            try 
            {
                var activeCode = await _authorizationCodeService.ValidateAuthorizationCode(verifyCodeRequestModel);

                if (activeCode != null)
                {
                    _logger.LogInformation($"Code {activeCode.Code} for PhoneNumber : {activeCode.Phone} Valid");
                    return Ok(new { Valid = true });
                }
                else 
                {
                    _logger.LogInformation($"No Active Codes were found with PhoneNumber : {verifyCodeRequestModel.PhoneNumber}, Please generate a new one");
                    return Ok("No valid codes were found..");
                }

            }
            catch (Exception ex) 
            {
                _logger.LogError($"VERIFY-CODE-ERROR: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }


    }
}
