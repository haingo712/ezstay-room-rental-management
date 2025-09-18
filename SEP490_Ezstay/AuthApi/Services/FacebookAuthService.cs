using AuthApi.DTO.Request;
using AuthApi.Models;
using AuthApi.Repositories.Interfaces;
using AuthApi.Services.Interfaces;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace AuthApi.Services
{
    public class FacebookAuthService : IFacebookAuthService
    {
        private readonly IAccountRepository _accountRepo;
        private readonly FacebookAuthSettings _fbSettings;
        private readonly HttpClient _http;

        public FacebookAuthService(IAccountRepository accountRepo, IOptions<FacebookAuthSettings> fbOptions)
        {
            _accountRepo = accountRepo;
            _fbSettings = fbOptions.Value;
            _http = new HttpClient();
        }

        public async Task<Account> FacebookLoginAsync(string accessToken)
        {
            // Lấy thông tin người dùng từ Facebook
            var url = $"https://graph.facebook.com/me?fields=id,name,email&access_token={accessToken}";
            var response = await _http.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                throw new Exception("Facebook token không hợp lệ");

            var content = await response.Content.ReadAsStringAsync();
            var fbData = JsonSerializer.Deserialize<FacebookUserData>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (fbData == null || string.IsNullOrEmpty(fbData.Email))
                throw new Exception("Không thể lấy email từ Facebook");

            var existingUser = await _accountRepo.GetByEmailAsync(fbData.Email);
            if (existingUser != null)
                return existingUser;

            throw new Exception("Tài khoản chưa đăng ký");
        }

    }
}
