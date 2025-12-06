using System.ComponentModel.DataAnnotations;

namespace AuthApi.DTO.Request
{
    public class UpdateEmailRequest
    {
        [Required]
        [EmailAddress]
        public string OldEmail { get; set; }
        [Required]
        [EmailAddress]
        public string NewEmail { get; set; }
    }
}
