using System.ComponentModel.DataAnnotations;
using UtilityBillAPI.Enum;

namespace UtilityBillAPI.DTO.Request
{
    public class UpdateUtilityBillDTO
    {        
      /*  [Required]
        public DateTime DueDate { get; set; }  */              
        [StringLength(500, ErrorMessage = "Note cannot exceed 500 characters.")]
        public string? Note { get; set; }        
    }
}
