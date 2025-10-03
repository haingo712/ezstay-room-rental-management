using AccountAPI.DTO.Response;
using AccountAPI.Service.Interfaces;

namespace AccountAPI.Service
{
    public class AuthApiClient : IAuthApiClient
    {
        private readonly HttpClient _http;
    

        public AuthApiClient(HttpClient http, IHttpClientFactory factory)
        {
            _http = http;
            _http = factory.CreateClient("Gateway");


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

        public async Task<AccountResponse?> GetByIdAsync(Guid id)
        {
            var response = await _http.GetAsync($"/api/Accounts/{id}");
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<AccountResponse>();
        }

        public async Task<bool> UpdateFullNameAsync(Guid id, string fullName)
        {
            var response = await _http.PutAsJsonAsync($"/api/Accounts/update-fullname/{id}", fullName);
            return response.IsSuccessStatusCode;
        }


    }
}
