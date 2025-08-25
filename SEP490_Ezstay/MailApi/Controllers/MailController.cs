using MailKit.Net.Smtp;
using MimeKit;
using MailApi.DTOs.Request;
using Microsoft.AspNetCore.Mvc;

namespace MailApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
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
    }
}