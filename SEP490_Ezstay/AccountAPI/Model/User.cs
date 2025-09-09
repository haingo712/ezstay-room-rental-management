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

        public string Adrress {get; set;} 

        public GenderEnum Gender { get; set; } 

        public string Avata {get; set;}

        public string Bio {get; set;}
        public DateTime DateOfBirth { get; set;}
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
    }
}
