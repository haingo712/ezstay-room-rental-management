using AuthApi.Models;

namespace AuthApi.Services.Interfaces
{
    public interface IGoogleAuthService
    {
        Task<Account> GoogleLoginAsync(string idToken);
    }
}
