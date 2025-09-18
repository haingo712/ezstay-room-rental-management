using AccountAPI.Service.Interfaces;

namespace AccountAPI.Service
{
    public class PhoneOtpClient : IPhoneOtpClient
    {
        private readonly HttpClient _http;

        public PhoneOtpClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<bool> SendOtpAsync(string phone)
        {
            var res = await _http.PostAsJsonAsync("/api/Auth/send-phone-otp", new { phone });
            return res.IsSuccessStatusCode;
        }

        public async Task<bool> VerifyOtpAsync(string phone, string otp)
        {
            var res = await _http.PostAsJsonAsync("/api/Auth/verify-phone-otp", new { phone, otp });
            return res.IsSuccessStatusCode;
        }
    }
}
