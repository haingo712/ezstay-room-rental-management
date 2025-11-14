using AuthApi.DTO.Request;

using AuthApi.Models;
using AuthApi.Repositories.Interfaces;
using AuthApi.Services.Interfaces;
using Microsoft.Extensions.Options;
using System.Data;
using System.Text.Json;

namespace AuthApi.Services
{
    public class FacebookAuthService : IFacebookAuthService
    {
        private readonly IAuthRepository _accountRepo;
        private readonly FacebookAuthSettings _fbSettings;
        private readonly HttpClient _http;

        public FacebookAuthService(IAuthRepository accountRepo, IOptions<FacebookAuthSettings> fbOptions)
        {
            _accountRepo = accountRepo;
            _fbSettings = fbOptions.Value;
            _http = new HttpClient();
        }

        public async Task<Account> FacebookLoginAsync(string accessToken)
        {
            var url = $"https://graph.facebook.com/me?fields=id,name,email&access_token={accessToken}";
            var response = await _http.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                throw new Exception("Facebook token không hợp lệ");

            var content = await response.Content.ReadAsStringAsync();
            var fbData = JsonSerializer.Deserialize<FacebookUserData>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (fbData == null)
                throw new Exception("Không thể lấy thông tin người dùng từ Facebook");

            if (string.IsNullOrEmpty(fbData.Email))
                fbData.Email = $"{fbData.Id}@facebook.com"; // fallback email

            var existingUser = await _accountRepo.GetByEmailAsync(fbData.Email);
            if (existingUser != null)
                return existingUser;

            var newUser = new Account
            {
                FullName = fbData.Name ?? "Facebook User",
                Email = fbData.Email,
                Password = string.Empty,
                Phone = string.Empty,
                Role = Shared.Enums.RoleEnum.User,
                IsVerified = true,
                CreateAt = DateTime.UtcNow
            };

            await _accountRepo.CreateAsync(newUser);
            return newUser;
        }


    }
}
