using System.ComponentModel.DataAnnotations;

namespace ChatAPI.DTO.Request;

public class CreateChatMessage
{
    public string? Content { get; set; }
    public IFormFileCollection? Image { get; set; }
}