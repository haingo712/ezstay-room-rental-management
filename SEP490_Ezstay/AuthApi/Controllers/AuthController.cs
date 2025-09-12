using AuthApi.DTO.Request;
using AuthApi.DTO.Response;
using AuthApi.Repositories.Interfaces;
using AuthApi.Services;
using AuthApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AuthApi.Models;
using AutoMapper;
using System.Text.Json;

namespace AuthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IEmailVerificationService _emailVerificationService;
        private readonly IAccountRepository _accountRepo;
        private readonly IMapper _mapper;


        public AuthController(
            IAuthService authService,
            IEmailVerificationService emailVerificationService,
            IAccountRepository accountRepo,
            IMapper mapper)
        {
            _authService = authService;
            _emailVerificationService = emailVerificationService;
            _accountRepo = accountRepo;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<ActionResult<RegisterResponseDto>> Register([FromBody] RegisterRequestDto dto)
        {
            var result = await _authService.RegisterAsync(dto);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto dto)
        {
            var result = await _authService.LoginAsync(dto);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }


        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromBody] OtpVerifyRequest dto)
        {
            var verification = await _emailVerificationService.ConfirmOtpAsync(dto.Email, dto.Otp);
            if (verification == null || string.IsNullOrEmpty(verification.UserPayload))
            {
                return BadRequest(new { Success = false, Message = "Invalid or expired OTP." });
            }

            var registerDto = JsonSerializer.Deserialize<RegisterRequestDto>(verification.UserPayload);
            if (registerDto == null)
            {
                return BadRequest(new { Success = false, Message = "Could not retrieve registration data." });
            }

            // Check again if email or phone already exists, in case another registration completed
            var existingEmail = await _accountRepo.GetByEmailAsync(registerDto.Email);
            if (existingEmail != null)
                return BadRequest(new { Success = false, Message = "Email already exists" });

            var existingPhone = await _accountRepo.GetByPhoneAsync(registerDto.Phone);
            if (existingPhone != null)
                return BadRequest(new { Success = false, Message = "Phone already exists" });


            var account = _mapper.Map<Account>(registerDto);
            account.Password = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);
            account.IsVerified = true;
            account.Role = Enums.RoleEnum.User; // Default role

            await _accountRepo.CreateAsync(account);

            return Ok(new { Success = true, Message = "Email verified and account created successfully!" });
        }

        [HttpPost("resend-otp")]
        public async Task<IActionResult> ResendOtp([FromBody] ResendOtpRequestDto dto)
        {
            var result = await _authService.ResendOtpAsync(dto.Email);
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("create-staff")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<RegisterResponseDto>> CreateStaff([FromBody] CreateStaffRequestDto dto)
        {
            var result = await _authService.CreateStaffAsync(dto);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDto dto)
        {
            var result = await _authService.ForgotPasswordAsync(dto);
            return Ok(result);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto dto)
        {
            var result = await _authService.ResetPasswordAsync(dto);
            return Ok(result);
        }

    }
}
