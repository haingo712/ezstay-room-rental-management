using AccountAPI.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AccountAPI.Data
{
    public class User
    {
        [BsonId]
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid Id { get; set; } = Guid.NewGuid();

     

        public GenderEnum Gender { get; set; } 

        public string Avatar { get; set; }

        public string Bio {get; set;}
        public DateTime DateOfBirth { get; set;}
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }

        public string? Province { get; set; }   // Tên tỉnh/thành
        public string? Commune { get; set; }    // Tên xã/phường
        public string? DetailAddress { get; set; }
    }   
}
