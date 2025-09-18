    using AuthApi.Data;
    using AuthApi.Models;
    using AuthApi.Services.Interfaces;
    using Microsoft.Extensions.Options;
    using MongoDB.Driver;
    using Twilio;
    using Twilio.Rest.Api.V2010.Account;

    namespace AuthApi.Services
    {
        public class TwilioPhoneVerificationService : IPhoneVerificationService
        {
            private readonly IMongoCollection<PhoneVerification> _collection;
            private readonly TwilioSettings _settings;

            public TwilioPhoneVerificationService(MongoDbService mongoService, IOptions<TwilioSettings> options)
            {
                _collection = mongoService.PhoneVerifications;
                _settings = options.Value;
                TwilioClient.Init(_settings.AccountSid, _settings.AuthToken);
            }

            public async Task SendOtpAsync(string phone)
            {
                var otp = new Random().Next(100000, 999999).ToString();

                var verification = new PhoneVerification
                {
                    Phone = phone,
                    Otp = otp,
                    ExpireAt = DateTime.UtcNow.AddMinutes(5)
                };

                await _collection.InsertOneAsync(verification);

                // Gửi SMS bằng Twilio Messaging Service
                var message = await MessageResource.CreateAsync(
          body: $"Your OTP code is {otp}",
          messagingServiceSid: _settings.MessagingServiceSid, // ← Dùng cái này
          to: new Twilio.Types.PhoneNumber(phone)              // ← Số điện thoại người nhận (phải bắt đầu bằng +84)
      );


                Console.WriteLine($"SMS sent: SID={message.Sid}");
            }


            public async Task<bool> VerifyOtpAsync(string phone, string otp)
            {
                var verification = await _collection
                    .Find(v => v.Phone == phone && v.Otp == otp && v.ExpireAt > DateTime.UtcNow)
                    .FirstOrDefaultAsync();

                if (verification == null) return false;

                await _collection.DeleteOneAsync(v => v.Id == verification.Id);
                return true;
            }
        }
    }
