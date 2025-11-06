using MailApi.Config;
using MailApi.Model;
using MailApi.Repository.Interface;
using MailApi.Services.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace MailApi.Services;

public class MailService(
    IMailRepository _mailRepository,
    IOptions<MailSettings> _mailOptions) : IMailService
{
    
    public async Task<(bool success, string message)> VerifyOtp(string email, string otp)
    {
        var otpRecord = await _mailRepository.GetByEmailAndCodeAsync(email, otp);
        if (otpRecord == null) return (false, "OTP not found");
        if (otpRecord.IsUsed) return (false, "OTP already used");
        if (DateTime.UtcNow > otpRecord.ExpireAt) return (false, "OTP expired");

        otpRecord.IsUsed = true;
        await _mailRepository.Update(otpRecord);
        return (true, "OTP verified successfully");
    }

    public async Task SendOtp(string email, Guid? contractId)
    {
        var otpCode = new Random().Next(100000, 999999).ToString();
        var otp = new OtpVerification
        {
            Email = email,
            OtpCode = otpCode,
            ExpireAt = DateTime.UtcNow.AddMinutes(5),
            ContractId = contractId ?? Guid.Empty
        };

        await _mailRepository.AddAsync(otp);
        await SendEmail(email, "EzStay - Your OTP Code",
            $"Your OTP code is: {otpCode}. It expires in 5 minutes.");
    }
    
    public async Task SendEmail(string toEmail, string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_mailOptions.Value.DisplayName, _mailOptions.Value.From));
        message.To.Add(new MailboxAddress("", toEmail));
        message.Subject = subject;

        message.Body = new TextPart("plain") { Text = body };

        using var client = new SmtpClient();
        client.ServerCertificateValidationCallback = (s, c, h, e) => true;

        await client.ConnectAsync(_mailOptions.Value.Host, _mailOptions.Value.Port, _mailOptions.Value.UseSSL);
        await client.AuthenticateAsync(_mailOptions.Value.UserName, _mailOptions.Value.Password);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}