
using AuthApi.Models;
using AuthApi.Repositories.Interfaces;
using AuthApi.Services.Interfaces;
using AuthApi.Utils;
using AutoMapper;
using System.Net.Http.Json;
using BCrypt.Net;
using System.Security.Claims;

using AuthApi.DTO.Response;
using AuthApi.DTO.Request;
using Shared.DTOs.Auths.Responses;


namespace AuthApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _repo;
        private readonly IMapper _mapper;
        private readonly HttpClient _httpClient;
        private readonly IEmailVerificationService _emailVerificationService;
        private readonly GenerateJwtToken _tokenGenerator;
        private readonly IPhoneVerificationService _phoneVerificationService;

        public AuthService(
            IAuthRepository repo,
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

        public async Task<Shared.DTOs.Auths.Responses.LoginResponseDto> LoginAsync(LoginRequestDto dto)
        {
            var account = await _repo.GetByEmailAsync(dto.Email);

            if (account == null)
                return new Shared.DTOs.Auths.Responses.LoginResponseDto { Success = false, Message = "Account not found." };
            if (!BCrypt.Net.BCrypt.Verify(dto.Password, account.Password))
                return new Shared.DTOs.Auths.Responses.LoginResponseDto { Success = false, Message = "Incorrect password." };
            if(account.IsBanned == true)
                return new Shared.DTOs.Auths.Responses.LoginResponseDto { Success = false, Message = "Account has been banned." };  
            if (!account.IsVerified)
                return new Shared.DTOs.Auths.Responses.LoginResponseDto { Success = false, Message = "Account has not been verified." };


            // ✅ Tạo token
            var token = _tokenGenerator.CreateToken(
             
                role: account.Role.ToString(),
                userId: account.Id.ToString()
            );

            return new Shared.DTOs.Auths.Responses.LoginResponseDto
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
            account.Role = Shared.Enums.RoleEnum.Staff;
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

            // Gửi OTP
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

        public async Task<RegisterResponseDto> ConfirmOtpForForgotPasswordAsync(string email, string otp)
        {
            var verification = await _emailVerificationService.ConfirmOtpAsync(email, otp);
            if (verification == null || verification.ExpiredAt < DateTime.UtcNow)
                return new RegisterResponseDto { Success = false, Message = "Invalid or expired OTP." };

            verification.IsVerifiedForReset = true;
            verification.VerifiedAt = DateTime.UtcNow;
            await _emailVerificationService.UpdateVerificationAsync(verification);

            return new RegisterResponseDto
            {
                Success = true,
                Message = "OTP verified. You can now reset your password."
            };
        }

        public async Task<RegisterResponseDto> ResetPasswordAsync(ResetPasswordRequestDto dto)
        {
            var account = await _repo.GetByEmailAsync(dto.Email);
            if (account == null)
                return new RegisterResponseDto { Success = false, Message = "Account not found." };

            var verification = await _emailVerificationService.GetVerificationByEmail(dto.Email);
            if (verification == null || !verification.IsVerifiedForReset ||
                verification.VerifiedAt == null ||
                verification.VerifiedAt.Value.AddMinutes(10) < DateTime.UtcNow)
            {
                return new RegisterResponseDto { Success = false, Message = "OTP not verified or expired." };
            }

            account.Password = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            await _repo.UpdateAsync(account);

            verification.IsVerifiedForReset = false;
            verification.VerifiedAt = null;
            await _emailVerificationService.UpdateVerificationAsync(verification);

            return new RegisterResponseDto
            {
                Success = true,
                Message = "Password has been reset successfully."
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
