using AuthApi.DTO.Request;
using AuthApi.DTO.Response;
using AuthApi.Models;
using AuthApi.Repositories.Interfaces;
using AuthApi.Services.Interfaces;
using AuthApi.Utils;
using AutoMapper;
using System.Net.Http.Json;

namespace AuthApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAccountRepository _repo;
        private readonly IMapper _mapper;
        private readonly HttpClient _httpClient;
        private readonly IEmailVerificationService _emailVerificationService;
        private readonly GenerateJwtToken _tokenGenerator;


        public AuthService(IAccountRepository repo, IMapper mapper, IHttpClientFactory factory, IEmailVerificationService emailVerificationService,GenerateJwtToken generateJwtToken)
        {
            _repo = repo;
            _mapper = mapper;
            _httpClient = factory.CreateClient("MailApi");
            _emailVerificationService = emailVerificationService;
            _tokenGenerator = generateJwtToken;
        }

        public async Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto dto)
        {
            var existingEmail = await _repo.GetByEmailAsync(dto.Email);
            if (existingEmail != null)
                return new RegisterResponseDto { Success = false, Message = "Email already exists" };

            var existingPhone = await _repo.GetByPhoneAsync(dto.Phone);
            if (existingPhone != null)
                return new RegisterResponseDto { Success = false, Message = "Phone already exists" };

            // ❌ Không còn gọi verify-email giả lập nữa

            // ✅ Tạo tài khoản nhưng chưa xác minh
            var account = _mapper.Map<Account>(dto);
            account.IsVerified = false; // thêm thuộc tính này trong model Account
            await _repo.CreateAsync(account);

            // ✅ Gửi OTP qua email
            await _emailVerificationService.SendOtpAsync(dto.Email);

            return new RegisterResponseDto
            {
                Success = true,
                Message = "OTP sent to email. Please verify to complete registration"
            };
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto dto)
        {
            var account = await _repo.GetByEmailAsync(dto.Email);

            if (account == null)
                return new LoginResponseDto { Success = false, Message = "Email not found" };

            if (!account.IsVerified)
                return new LoginResponseDto { Success = false, Message = "Email not verified" };

            if (account.Password != dto.Password)
                return new LoginResponseDto { Success = false, Message = "Invalid password" };

            // ✅ Tạo token
            var token = _tokenGenerator.CreateToken(
                email: account.Email,
                role: account.Role.ToString(),
                userId: account.Id.ToString()
            );

            return new LoginResponseDto
            {
                Success = true,
                Token = token,
                Message = "Login successful"
            };
        }




    }


}
