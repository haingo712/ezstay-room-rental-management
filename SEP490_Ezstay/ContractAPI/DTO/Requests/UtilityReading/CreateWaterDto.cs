using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ContractAPI.Enum;

namespace ContractAPI.DTO.Requests.UtilityReading;

public class CreateWaterDto
{
    public decimal Price { get; set; }
    [StringLength(100, ErrorMessage = "Note cannot exceed 100 characters.")]
    public string Note { get; set; }
    public decimal CurrentIndex { get; set;}
}