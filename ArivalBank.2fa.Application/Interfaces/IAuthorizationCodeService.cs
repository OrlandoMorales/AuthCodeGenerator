using ArivalBank._2fa.Application.Models;
using ArivalBank._2fa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArivalBank._2fa.Application.Interfaces
{
    public interface IAuthorizationCodeService
    {
        Task<AuthorizationCode> GenerateAuthorizationCode(AuthorizationCodeRequestModel authenticationCodeRequestModel);
        Task<AuthorizationCode> ValidateAuthorizationCode(VerifyCodeRequestModel verifyCodeRequestModel);
        Task<int> GetTotalActiveCodes(AuthorizationCodeRequestModel authenticationCodeRequestModel);
        bool IsCodeExpired(DateTime expirationTime);
        string GenerateCode();
    }
}