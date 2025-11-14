using SupportAPI.Enums;

namespace SupportAPI.DTO.Request
{
    public class UpdateSupportStatusRequest
    {
        public StatusEnums Status { get; set; }
    }
}
