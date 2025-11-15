namespace ServiceAPI.DTO.Response
{
    public class ServiceItemResponseDto
    {
        public Guid Id { get; set; }
        public string ServiceName { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid OwnerId { get; set; }
    }
}
