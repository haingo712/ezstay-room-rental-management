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

            // Lấy thông tin từ Google
            var email = payload.Email;
            var name = payload.Name;

            // Kiểm tra tài khoản có tồn tại chưa
            var existingUser = await _userRepo.GetByEmailAsync(email);
            if (existingUser != null)
            {
                return existingUser;
            }

            // 👉 Nếu chưa có thì tạo tài khoản mới
            var newUser = new Account
            {
                FullName = name,
                Email = email,
                Password = string.Empty, // login google không cần mật khẩu
                Phone = string.Empty,
                Role = RoleEnum.User, // hoặc 1 role mặc định bạn chọn
                IsVerified = true, // xác minh luôn vì Google đã verify email
                CreateAt = DateTime.UtcNow
            };

            await _userRepo.AddAsync(newUser);

            return newUser;
        }

    }
}
