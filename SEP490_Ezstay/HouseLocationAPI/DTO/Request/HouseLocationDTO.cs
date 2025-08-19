namespace HouseLocationAPI.DTO.Request
{
    public class HouseLocationDTO
    {        
        public Guid Id { get; set; }
        public Guid HouseId { get; set; }        
        public string FullAddress { get; set; } = null!;
    }
}
