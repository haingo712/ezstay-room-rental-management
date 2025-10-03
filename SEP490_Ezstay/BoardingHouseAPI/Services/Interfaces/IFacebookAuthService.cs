using AuthApi.Models;

namespace AuthApi.Services.Interfaces
{
    public interface IFacebookAuthService
    {
        Task<Account> FacebookLoginAsync(string accessToken);
    }
}
