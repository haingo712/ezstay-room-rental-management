using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Shared.Enums;

namespace AuthApi.Models
{
    public class Account
    {
        [BsonId]
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string FullName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public DateTime CreateAt {get; set; } = DateTime.UtcNow;

        public RoleEnum Role { get; set; } = RoleEnum.User;

        public bool IsVerified { get; set; } = false;

        public bool IsBanned { get; set; } = false;



    }

}
