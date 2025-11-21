using System.ComponentModel.DataAnnotations;
using Shared.DTOs.Auths.Responses;
using Shared.DTOs.RentalPosts.Responses;

namespace Shared.DTOs.Chats.Responses;

public class ChatRoomResponse
{
    public Guid Id { get; set; } 
    public Guid OwnerId { get; set; } 
    public Guid UserId { get; set; } 
    public ChatUserInfoResponse User { get; set; }
    public ChatUserInfoResponse Owner { get; set; }
}