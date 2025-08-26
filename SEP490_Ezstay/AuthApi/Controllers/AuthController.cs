using AuthApi.DTO.Request;
using AuthApi.DTO.Response;
using AuthApi.Repositories.Interfaces;
using AuthApi.Services;
using AuthApi.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IEmailVerificationService _emailVerificationService;
        private readonly IAccountRepository _accountRepo;


        public AuthController(
            IAuthService authService,
            IEmailVerificationService emailVerificationService,
            IAccountRepository accountRepo)
        {
            _authService = authService;
            _emailVerificationService = emailVerificationService;
            _accountRepo = accountRepo;
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


        /*[HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromBody] OtpVerifyRequest dto)
        {
            var valid = await _emailVerificationService.ConfirmOtpAsync(dto.Email, dto.Otp);
            if (!valid) return BadRequest("Invalid or expired OTP");

            await _accountRepo.MarkAsVerified(dto.Email);
            return Ok("Email verified successfully");
        }*/


    }
}
