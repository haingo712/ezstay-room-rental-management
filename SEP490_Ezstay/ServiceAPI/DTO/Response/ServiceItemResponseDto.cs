namespace ServiceAPI.DTO.Response
{
    public class ServiceItemResponseDto
    {
        public string Id { get; set; }
        public string ServiceName { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
