using AccountAPI.Service.Interfaces;

namespace AccountAPI.Service
{
    public class AuthApiClient : IAuthApiClient
    {
        private readonly HttpClient _http;

        public AuthApiClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<bool> ConfirmOtpAsync(string email, string otp)
        {
            var response = await _http.PostAsJsonAsync("/api/Auth/confirm-otp", new
            {
                Email = email,
                Otp = otp
            });

            return response.IsSuccessStatusCode;
        }
        public async Task<bool> UpdateEmailAsync(string oldEmail, string newEmail)
        {
            var response = await _http.PutAsJsonAsync("/api/Auth/update-email", new
            {
                OldEmail = oldEmail,
                NewEmail = newEmail
            });

            return response.IsSuccessStatusCode;
        }

    }
}
