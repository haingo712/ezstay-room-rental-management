using System.ComponentModel.DataAnnotations;
using Shared.DTOs.Accounts.Responses;
using Shared.DTOs.RentalPosts.Responses;

namespace Shared.DTOs.Chats.Responses;

public class ChatRoomResponse
{
    public Guid Id { get; set; } 
    
    public Guid PostId { get; set; } 
    
    public Guid OwnerId { get; set; } 
    
    public Guid TenantId { get; set; } 
    public DateTime CreatedAt { get; set; } 
    public DateTime? LastMessageAt { get; set; } 
    public AccountResponse User { get; set; }
    public AccountResponse Owner { get; set; }
    public RentalPostResponse? RentalPost { get; set; }
}