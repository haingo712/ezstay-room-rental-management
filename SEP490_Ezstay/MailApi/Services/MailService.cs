using System.Text.Json;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MailApi.APIs;
using MailApi.APIs;
using MailApi.Config;
using MailApi.DTOs.Request;
using MailApi.Model;
using MailApi.Repository.Interface;
using MailApi.Services.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MimeKit;
using Shared.DTOs;
using Shared.DTOs.Contracts.Responses;
using Shared.DTOs.Mails.Response;
using Shared.Enums;

namespace MailApi.Services;

public class MailService(
    IMailRepository _mailRepository,
    IContractAPI _contractAPI,
    IOptions<MailSettings> _mailOptions) : IMailService
{
    
    public async Task<(bool success, string message)> VerifyOtp(Guid id, string otp, Guid signerId)
    {  
        // var contract = await _contractAPI.GetContractByIdAsync(contractId);
        // if (contract == null)
        //     return (false, "Contract not found");

        var mailId = await _mailRepository.GetByIdAsync(id);
        
        if (mailId == null)
            return (false, "11 not found");
        // var otpRecord = await _mailRepository.GetByContractAndSignerId(id, otp, signerId);
        //
        // if (otpRecord == null) 
        //     return (false, "OTP not found for this contract");
        //
        // if (!string.Equals(otpRecord.Email, email, StringComparison.OrdinalIgnoreCase))
        //     return (false, "Email does not match OTP record");
        //
        // if (otpRecord.IsUsed) 
        //     return (false, "OTP already used");
        //     
        // if (DateTime.UtcNow > otpRecord.ExpireAt)
        //     return (false, "OTP expired. Please request a new one.");
        
        //otpRecord.IsUsed = true;
        mailId.IsUsed = true;
        await _mailRepository.Update(mailId);
        return (true, $"OTP verified successfully for ");
    }

    public async Task<(bool success, string message, Guid? otpId)> SendOtp(string email, Guid contractId, Guid signerId)
    {
        // Get contract details to determine who should receive this OTP
        // var contract = await _contractAPI.GetContractByIdAsync(contractId);
        // if (contract == null)
        //     return (false, "Contract not found");
        //
        var otpCode = new Random().Next(100000, 999999).ToString();
        var otp = new OtpVerification
        {
            Email = email,
            OtpCode = otpCode,
            ExpireAt = DateTime.UtcNow.AddMinutes(5),
            ContractId = contractId,
            SignerId = signerId
        };
        
        var cr= await _mailRepository.AddAsync(otp);
        await SendEmail(email, $"EzStay - OTP for Contract Signing )",
            $"Your OTP code is: {otpCode}. This is for  to sign the contract. It expires in 5 minutes.");
        
        return (true, $"OTP sent successfully to{email} ",cr.Id);
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
    


    // public async Task<ApiResponse<OtpResponse>> Add(Guid ownerId, VerificationEmailContractRequest request)
    // {
    //     var mail = _mapper.Map<OtpVerification>(request);
    //     mail.ExpireAt = DateTime.UtcNow;
    //     var createdMail = await _mailRepository.AddAsync(mail);
    //     var dto = _mapper.Map<OtpResponse>(createdMail);
    //     return ApiResponse<OtpResponse>.Success(dto, "Tạo hợp đồng thuê thành công");
    // }
    

   
}