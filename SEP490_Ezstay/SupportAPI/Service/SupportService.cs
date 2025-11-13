using AutoMapper;
using MimeKit;
using SupportAPI.DTO.Request;
using SupportAPI.DTO.Response;
using SupportAPI.Model;
using SupportAPI.Repositories.Interfaces;
using SupportAPI.Service.Interfaces;


namespace SupportAPI.Service
{
    public class SupportService : ISupportService
    {
    
        private readonly IMapper _mapper;
        private readonly ISuportRepository _repository;
        public SupportService(ISuportRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<SupportResponse>> GetAllAsync()
        {
            var supports = await _repository.GetAllAsync();
            return _mapper.Map<List<SupportResponse>>(supports);
        }

        public async Task<SupportResponse> CreateAsync(CreateSupportRequest request)
        {
            var support = _mapper.Map<SupportModel>(request);
            await _repository.CreateAsync(support);
            return _mapper.Map<SupportResponse>(support);
        }

        public async Task<SupportResponse> UpdateStatusAsync(Guid id, UpdateSupportStatusRequest request)
        {
            var support = await _repository.GetByIdAsync(id);
            if (support == null) throw new Exception("Support not found");

            support.status = request.Status;
            await _repository.UpdateAsync(support);

            // ✅ Gửi mail khi update status
            await SendMailAsync(support.Email, support.Subject, request.Status.ToString());

            return _mapper.Map<SupportResponse>(support);
        }

        private async Task SendMailAsync(string toEmail, string subject, string status)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("EzStay Support", "qui4982@gmail.com"));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = "Cập nhật trạng thái hỗ trợ";

            message.Body = new TextPart("plain")
            {
                Text = $"Xin chào,\n\nYêu cầu hỗ trợ '{subject}' đã được cập nhật thành trạng thái: {status}.\n\nTrân trọng,\nEzStay Team"
            };

            using var client = new MailKit.Net.Smtp.SmtpClient();

            client.ServerCertificateValidationCallback = (s, c, h, e) => true;

            await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync("qui4982@gmail.com", "mjzs ixor nlgb udiz");
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }

    }
}
