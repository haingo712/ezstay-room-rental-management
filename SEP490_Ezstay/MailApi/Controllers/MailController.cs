using MailApi.DTOs.Request;
using MailApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MailApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController(IMailService _mailService) : ControllerBase
    {
        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOtp([FromBody] VerificationEmailContractRequest request)
        {
            await _mailService.SendOtp(request.Email, request.ContractId);
            return Ok(new { success = true, message = "OTP sent successfully" });
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerificationEmailContractRequest request)
        {
            var (success, message) = await _mailService.VerifyOtp(request.Email, request.Otp);
            if (!success) return BadRequest(new { success, message });
            return Ok(new { success, message });
        }
    }
}