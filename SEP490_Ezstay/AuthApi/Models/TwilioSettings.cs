using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace AuthApi.Models
{
    public class TwilioSettings
    {
        public string AccountSid { get; set; } = null!;
        public string AuthToken { get; set; } = null!;
        public string MessagingServiceSid { get; set; } = null!;
    }
}
