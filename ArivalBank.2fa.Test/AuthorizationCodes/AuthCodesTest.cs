using ArivalBank._2fa.Application.Authorization;
using ArivalBank._2fa.Application.Interfaces;
using ArivalBank._2fa.Application.Models;
using ArivalBank._2fa.Domain.Configuration;
using ArivalBank._2fa.Domain.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Linq;
using System.Text;

namespace ArivalBank._2fa.Test.AuthorizationCodes
{
    [TestClass]
    public class AuthCodesTest
    {
        AuthorizationCodeService _authorizationCodeService;
        IOptions<AppConfiguration> _appConfigurationMock;
        AuthorizationCode _authCode;

        [TestInitialize]
        public void Init()
        {
            var _loggerMock = Mock.Of<ILogger<AuthorizationCodeService>>();
            
            _appConfigurationMock = Options.Create(new AppConfiguration 
            {
                CodeDurationMinutes = 10,
                CodeLength = 6,
                IsSmsActive = false,
                MaxCodesByPhone = 5
            });

            _authCode = new AuthorizationCode
            {
                Code = "123ABC",
                DateCreated = DateTime.Now,
                ExpirationTime = DateTime.Now.AddMinutes(_appConfigurationMock.Value.CodeDurationMinutes),
                Phone = "5558987",
                Id = 1
            };

            Mock<IRepository<AuthorizationCode>> _repositoryMock = new Mock<IRepository<AuthorizationCode>>();
            Mock<ISmsGatewayMessaging> _smsGatewayMessagingMock = new Mock<ISmsGatewayMessaging>();
            Mock<IDistributedCache> _cacheMock = new Mock<IDistributedCache>();

            // Repository Setup
            var codesList = new List<AuthorizationCode>();
            codesList.Add(_authCode);
            _repositoryMock.Setup(s=> s.ListAll()).Returns(codesList);


           _authorizationCodeService = new AuthorizationCodeService(
               _loggerMock,
               _appConfigurationMock,
               _repositoryMock.Object,
               _smsGatewayMessagingMock.Object,
               _cacheMock.Object);
        }


        [TestMethod]
        public void Generate_Code_Configuration_Length_Test()
        {
            var code = _authorizationCodeService.GenerateCode();

            Assert.AreEqual(_appConfigurationMock.Value.CodeLength, code.Length);
        }


        [TestMethod]
        public void Generate_Auth_Code_Test()
        {
            var request = new AuthorizationCodeRequestModel();
            request.PhoneNumber = "5558987";

            var code = _authorizationCodeService.GenerateAuthorizationCode(request);

            Assert.AreEqual(request.PhoneNumber, code.Result.Phone);
        }


        [TestMethod]
        public void Validate_Auth_Code_Test()
        {
            var request = new VerifyCodeRequestModel();
            request.PhoneNumber = "5558987";
            request.Code = "123ABC";

            var code = _authorizationCodeService.ValidateAuthorizationCode(request);

            Assert.AreEqual(request.PhoneNumber, code.Result.Phone);
            Assert.AreEqual(request.Code, code.Result.Code);
        }

    }
}