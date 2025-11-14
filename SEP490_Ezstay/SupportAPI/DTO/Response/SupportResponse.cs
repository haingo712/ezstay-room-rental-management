using SupportAPI.Enums;

namespace SupportAPI.DTO.Response
{
    public class SupportResponse
    {
        public Guid Id { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public StatusEnums Status { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
