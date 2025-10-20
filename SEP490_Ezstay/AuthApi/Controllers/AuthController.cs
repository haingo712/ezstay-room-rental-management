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
        private readonly IGoogleAuthService _googleAuthService;
        private readonly IFacebookAuthService _facebookAuthService;


        public AuthController(
            IAuthService authService,
            IEmailVerificationService emailVerificationService,
            IAccountRepository accountRepo,
            IMapper mapper,
            IGoogleAuthService googleAuthService,
            IFacebookAuthService facebookAuthService)
        {
            _authService = authService;
            _emailVerificationService = emailVerificationService;
            _accountRepo = accountRepo;
            _mapper = mapper;
            _googleAuthService = googleAuthService;
            _facebookAuthService = facebookAuthService;
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

        [HttpPost("confirm-otp")]
        public async Task<IActionResult> ConfirmOtp([FromBody] ConfirmOtpRequest dto)
        {
            var result = await _authService.ConfirmOtpForForgotPasswordAsync(dto.Email, dto.Otp);
            if (!result.Success) return BadRequest(result);           
          

            return Ok(result);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto dto)
        {
            var result = await _authService.ResetPasswordAsync(dto);
            return Ok(result);
        }





        [HttpPut("update-email")]
        [Authorize]
        public async Task<IActionResult> UpdateEmail([FromBody] UpdateEmailRequest dto)
        {
            var account = await _accountRepo.GetByEmailAsync(dto.OldEmail);
            if (account == null)
                return NotFound("Account not found.");

            // Update email
            account.Email = dto.NewEmail;
            await _accountRepo.UpdateAsync(account);

            return Ok("Email updated successfully.");
        }

        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequest request)
        {
            try
            {
                var user = await _googleAuthService.GoogleLoginAsync(request.IdToken);

                return Ok(new
                {
                    message = "Đăng nhập thành công",
                    user = new
                    {
                        user.Id,
                        user.FullName,
                        user.Email,
                        user.Role
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpPost("facebook-login")]
        public async Task<IActionResult> FacebookLogin([FromBody] FacebookLoginRequest request)
        {
            try
            {
                var user = await _facebookAuthService.FacebookLoginAsync(request.AccessToken);

                return Ok(new
                {
                    message = "Login thành công",
                    user = new
                    {
                        user.Id,
                        user.FullName,
                        user.Email,
                        user.Role
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }



        [HttpPost("send-phone-otp")]
        public async Task<IActionResult> SendPhoneOtp([FromBody] PhoneVerificationRequestDto dto)
        {
            var result = await _authService.SendPhoneOtpAsync(dto.Phone);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("verify-phone-otp")]
        public async Task<IActionResult> VerifyPhoneOtp([FromBody] VerifyPhoneOtpRequestDto dto)
        {
            var result = await _authService.VerifyPhoneOtpAsync(dto.Phone, dto.Otp);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }





    }
}
