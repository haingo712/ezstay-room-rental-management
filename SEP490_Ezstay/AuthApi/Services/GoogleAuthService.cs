using AuthApi.DTO.Request;
using AuthApi.Enums;
using AuthApi.Models;
using AuthApi.Repositories.Interfaces;
using AuthApi.Services.Interfaces;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Extensions.Options;
using System.Web.Helpers;
using System.Xml.Linq;

namespace AuthApi.Services
{
    public class GoogleAuthService : IGoogleAuthService
    {
        private readonly IAccountRepository _userRepo;
        private readonly GoogleAuthSettings _googleSettings;

        public GoogleAuthService(IAccountRepository userRepo, IOptions<GoogleAuthSettings> googleOptions)
        {
            _userRepo = userRepo;
            _googleSettings = googleOptions.Value;
        }

        public async Task<Account> GoogleLoginAsync(string idToken)
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[] { _googleSettings.ClientId }
            });

            var email = payload.Email;

            var existingUser = await _userRepo.GetByEmailAsync(email);
            if (existingUser != null)
            {
                return existingUser;
            }

            // Không tạo tài khoản mới
            throw new Exception("Tài khoản chưa đăng ký");
        }
    }
}
