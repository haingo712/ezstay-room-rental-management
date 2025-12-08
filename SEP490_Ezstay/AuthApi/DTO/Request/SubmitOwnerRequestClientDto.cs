using System.ComponentModel.DataAnnotations;

namespace AuthApi.DTO.Request
{
    public class SubmitOwnerRequestClientDto
    {
        [Required]
        public string Reason { get; set; } 
        [Required]

        public IFormFile Imageasset { get; set; }

    }
}
