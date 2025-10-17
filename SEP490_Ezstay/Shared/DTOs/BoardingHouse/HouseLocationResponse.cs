namespace Shared.DTOs.BoardingHouse
{
    public class HouseLocationResponse
    {
        public Guid Id { get; set; }        
        public string ProvinceId { get; set; } = null!;
        public string ProvinceName { get; set; } = null!;
        public string CommuneId { get; set; } = null!;
        public string CommuneName { get; set; } = null!;
        public string AddressDetail { get; set; } = null!;
        public string FullAddress { get; set; } = null!;
    }
}
