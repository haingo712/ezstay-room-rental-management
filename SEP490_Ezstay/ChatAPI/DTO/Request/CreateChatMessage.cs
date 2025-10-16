using System.ComponentModel.DataAnnotations;

namespace ChatAPI.DTO.Request;

public class CreateChatMessage
{
   // public Guid SenderId { get; set; }
    [Required]
    public string Content { get; set; }
    public string? Image { get; set; }
}