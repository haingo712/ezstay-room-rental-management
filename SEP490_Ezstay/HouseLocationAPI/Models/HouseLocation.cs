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
        public string ProvinceId { get; set; } = null!;
        public string ProvinceName { get; set; } = null!;

        public string CommuneId { get; set; } = null!;
        public string CommuneName { get; set; } = null!;
        public string AddressDetail { get; set; } = null!;
        public string FullAddress { get; set; } = null!;

    }
}
