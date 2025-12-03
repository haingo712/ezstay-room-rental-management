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
    public class MailController : ControllerBase
    {
    private readonly IMailService _mailService;
    private readonly ITokenService _token;

    public MailController(IMailService mailService, ITokenService token)
    {
        _mailService = mailService;
        _token = token;
    }

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
        
        [HttpPost("send-otp/{contractId}")]
        public async Task<IActionResult> SendOtp(Guid contractId, [FromQuery] string email)
        {
            Guid signerId = _token.GetUserIdFromClaims(User);
            var result = await _mailService.SendOtp(email, contractId, signerId);
            if (!result.success) 
                return BadRequest(new { success = false, message = result.message });
            
            return Ok(new { success = true,   result.message, result.otpId });
        }
        
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