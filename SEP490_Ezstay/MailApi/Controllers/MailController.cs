using MailApi.DTOs.Request;
using MailApi.Services.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MimeKit;

namespace MailApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController(IMailService _mailService, ITokenService _token) : ControllerBase
    {
        [HttpPost("send-verification")]
        public IActionResult SendVerification([FromBody] VerificationEmailRequest request)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("EzStay", "qui4982@gmail.com"));
            message.To.Add(new MailboxAddress("", request.Email));
            message.Subject = "EzStay - Your OTP Code";

            message.Body = new TextPart("plain")
            {
                Text = $"Your OTP code is: {request.Otp}. It expires in 5 minutes."
            };

            using var client = new SmtpClient();

            // ✅ Fix SSL handshake error (nếu bị)
            client.ServerCertificateValidationCallback = (s, c, h, e) => true;

            client.Connect("smtp.gmail.com", 587, false);
            client.Authenticate("qui4982@gmail.com", "mjzs ixor nlgb udiz"); // ✅ App Password
            client.Send(message);
            client.Disconnect(true);

            return Ok(new { success = true, message = "OTP sent" });
        }
        // [HttpPost("send-otp")]
        // public async Task<IActionResult> SendOtp([FromBody] VerificationEmailContractRequest request)
        // {
        //     await _mailService.SendOtp(request.Email, request.ContractId);
        //     return Ok(new { success = true, message = "OTP sent successfully" });
        // }
        
        [HttpPost("send-otp/{contractId}")]
        public async Task<IActionResult> SendOtp(Guid contractId, [FromQuery] string email)
        {
            Guid signerId = _token.GetUserIdFromClaims(User);
            var result = await _mailService.SendOtp(email, contractId, signerId);
            if (!result.success) 
                return BadRequest(new { success = false, message = result.message });
            
            return Ok(new { success = true, message = result.message });
        }

        /// <summary>
        /// Verify OTP using ContractId and OTP code
        /// System will automatically determine if this is Owner or Tenant based on email
        /// </summary>
        // [HttpPost("verify-otp/{contractId}")]
        // public async Task<IActionResult> VerifyOtp(Guid contractId, [FromBody] VerificationEmailContractRequest request)
        // {
        //     var signerId = _token.GetUserIdFromClaims(User);
        //     var (success, message) = await _mailService.VerifyOtp(contractId, request.Otp, request.Email, signerId);
        //     if (!success) return BadRequest(new { success, message });
        //     return Ok(new { success, message });
        // }
        
        [HttpPut("verify-otp/{id}")]
        public async Task<IActionResult> VerifyOtp(Guid id, [FromBody] VerificationEmailContractRequest request)
        {
            var signerId = _token.GetUserIdFromClaims(User);
            var (success, message) = await _mailService.VerifyOtp(id, request.Otp, signerId);
            if (!success) return BadRequest(new { success, message });
            return Ok(new { success, message });
        }
    }
}