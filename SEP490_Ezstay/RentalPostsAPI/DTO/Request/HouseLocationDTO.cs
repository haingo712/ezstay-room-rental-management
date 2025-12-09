namespace RentalPostsAPI.DTO.Request
{
    public class HouseLocationDTO
    {
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
