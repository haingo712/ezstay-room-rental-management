using AuthApi.DTO.Request;
using AuthApi.DTO.Response;

namespace AuthApi.Services.Interfaces
{
    public interface IAuthService
    {
        Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto dto);
        Task<LoginResponseDto> LoginAsync(LoginRequestDto dto);
    }
}
