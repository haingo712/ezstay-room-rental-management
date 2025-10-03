using AuthApi.DTO.Request;
using AuthApi.DTO.Response;
using AuthApi.Models;
using AuthApi.Repositories.Interfaces;
using AuthApi.Services.Interfaces;
using AuthApi.Utils;
using AutoMapper;
using System.Net.Http.Json;
using BCrypt.Net;
using System.Security.Claims;

namespace AuthApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAccountRepository _repo;
        private readonly IMapper _mapper;
        private readonly HttpClient _httpClient;
        private readonly IEmailVerificationService _emailVerificationService;
        private readonly GenerateJwtToken _tokenGenerator;


        private readonly IPhoneVerificationService _phoneVerificationService;

        public AuthService(
            IAccountRepository repo,
            IMapper mapper,
            IHttpClientFactory factory,
            IEmailVerificationService emailVerificationService,
            GenerateJwtToken generateJwtToken,
            IPhoneVerificationService phoneVerificationService)  // thêm đây
        {
            _repo = repo;
            _mapper = mapper;
            _httpClient = factory.CreateClient("MailApi");
            _emailVerificationService = emailVerificationService;
            _tokenGenerator = generateJwtToken;
            _phoneVerificationService = phoneVerificationService; // lưu vào field
        }


        public async Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto dto)
        {
            var existingEmail = await _repo.GetByEmailAsync(dto.Email);
            if (existingEmail != null)
                return new RegisterResponseDto { Success = false, Message = "Email already exists" };

            var existingPhone = await _repo.GetByPhoneAsync(dto.Phone);
            if (existingPhone != null)
                return new RegisterResponseDto { Success = false, Message = "Phone already exists" };

            // Temporarily store user data and send OTP
            await _emailVerificationService.SendOtpAsync(dto);

            return new RegisterResponseDto
            {
                Success = true,
                Message = "OTP sent to email. Please verify to complete registration"
            };
        }

        public async Task<RegisterResponseDto> ResendOtpAsync(string email)
        {
            var verification = await _emailVerificationService.GetVerificationByEmail(email);
            if (verification == null || string.IsNullOrEmpty(verification.UserPayload))
                return new RegisterResponseDto { Success = false, Message = "Cannot find original registration data to resend OTP." };

            await _emailVerificationService.SendOtpAsync(verification.UserPayload);
            return new RegisterResponseDto
            {
                Success = true,
                Message = "OTP resent to email. Please check your inbox."
            };
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto dto)
        {
            var account = await _repo.GetByEmailAsync(dto.Email);

            if (account == null)
                return new LoginResponseDto { Success = false, Message = "Account not found." };

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, account.Password))
                return new LoginResponseDto { Success = false, Message = "Incorrect password." };

            if (!account.IsVerified)
                return new LoginResponseDto { Success = false, Message = "Account has not been verified." };


            // ✅ Tạo token
            var token = _tokenGenerator.CreateToken(
             
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

        public async Task<RegisterResponseDto> CreateStaffAsync(CreateStaffRequestDto dto)
        {
            var existingEmail = await _repo.GetByEmailAsync(dto.Email);
            if (existingEmail != null)
                return new RegisterResponseDto { Success = false, Message = "Email already exists" };

            var existingPhone = await _repo.GetByPhoneAsync(dto.Phone);
            if (existingPhone != null)
                return new RegisterResponseDto { Success = false, Message = "Phone already exists" };

            var account = _mapper.Map<Account>(dto);
            account.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            account.Role = AuthApi.Enums.RoleEnum.Staff;
            account.IsVerified = true; // Staff accounts are created by admin and are verified by default

            await _repo.CreateAsync(account);

            return new RegisterResponseDto
            {
                Success = true,
                Message = "Staff account created successfully"
            };
        }


        public async Task<RegisterResponseDto> ForgotPasswordAsync(ForgotPasswordRequestDto dto)
        {
            var account = await _repo.GetByEmailAsync(dto.Email);
            if (account == null)
                return new RegisterResponseDto { Success = false, Message = "Email not found." };

            // Gửi OTP (dùng lại hàm SendOtpAsync)
            await _emailVerificationService.SendOtpAsync(new RegisterRequestDto
            {
                Email = dto.Email,
                FullName = account.FullName,
                Phone = account.Phone,
            });

            return new RegisterResponseDto
            {
                Success = true,
                Message = "OTP sent to email. Please check your inbox."
            };
        }

        public async Task<RegisterResponseDto> ResetPasswordAsync(ResetPasswordRequestDto dto)
        {
            // Giải mã token để lấy email
            var principal = _tokenGenerator.ValidateToken(dto.Token);
            if (principal == null)
                return new RegisterResponseDto { Success = false, Message = "Invalid or expired token." };

            var email = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
                return new RegisterResponseDto { Success = false, Message = "Invalid token data." };

            var account = await _repo.GetByEmailAsync(email);
            if (account == null)
                return new RegisterResponseDto { Success = false, Message = "Account not found." };

            account.Password = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            await _repo.UpdateAsync(account);

            return new RegisterResponseDto
            {
                Success = true,
                Message = "Password reset successfully"
            };
        }


        public async Task<RegisterResponseDto> SendPhoneOtpAsync(string phone)
        {
            // Kiểm tra số điện thoại đã tồn tại trong DB chưa
            var existingPhone = await _repo.GetByPhoneAsync(phone);
            if (existingPhone != null)
                return new RegisterResponseDto { Success = false, Message = "Phone already exists" };

            // Gửi OTP thật bằng Twilio
            await _phoneVerificationService.SendOtpAsync(phone);

            return new RegisterResponseDto { Success = true, Message = "OTP sent to phone." };
        }

        public async Task<RegisterResponseDto> VerifyPhoneOtpAsync(string phone, string otp)
        {
            var verified = await _phoneVerificationService.VerifyOtpAsync(phone, otp);

            if (!verified)
                return new RegisterResponseDto { Success = false, Message = "Invalid or expired OTP." };

            // Nếu muốn, bạn có thể đánh dấu account đã verify phone
            var account = await _repo.GetByPhoneAsync(phone);
            if (account != null)
            {
                account.IsVerified = true;
                await _repo.UpdateAsync(account);
            }

            return new RegisterResponseDto { Success = true, Message = "Phone verified successfully." };
        }









    }


}
