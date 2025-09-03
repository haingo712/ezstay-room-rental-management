using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HouseLocationAPI.Models
{
    public class HouseLocation
    {
        [BsonId]
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid Id { get; set; }
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid HouseId { get; set; }
        public string FullAddress { get; set; } = null!;

    }
}
