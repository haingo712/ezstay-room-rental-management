namespace SupportAPI.DTO.Request
{
    public class CreateSupportRequest
    {
        public string Subject { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
    }
}
