using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BoardingHouseAPI.Models
{
    public class HouseLocation
    {
        [BsonId]
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid Id { get; set; }        
        public string ProvinceId { get; set; } = null!;
        public string ProvinceName { get; set; } = null!;
        
        public string DistrictId { get; set; } = null!;
        public string DistrictName { get; set; } = null!;

        public string CommuneId { get; set; } = null!;
        public string CommuneName { get; set; } = null!;
        public string AddressDetail { get; set; } = null!;
        public string FullAddress { get; set; } = null!;

    }
}
