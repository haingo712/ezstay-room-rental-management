using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Chats.Responses;

public class ChatRoomResponse
{
    public Guid Id { get; set; } 
    
    public Guid PostId { get; set; } 
    
    public Guid OwnerId { get; set; } 
    
    public Guid TenantId { get; set; } 
    public DateTime CreatedAt { get; set; } 
    public DateTime? LastMessageAt { get; set; } 
}