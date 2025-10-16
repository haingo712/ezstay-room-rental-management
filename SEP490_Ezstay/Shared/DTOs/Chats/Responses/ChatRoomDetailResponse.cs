using Shared.DTOs.Accounts.Responses;
using Shared.DTOs.RentalPosts.Responses;

namespace Shared.DTOs.Chats.Responses;

public class ChatRoomDetailResponse
{
    public Guid RoomId { get; set; }
        
    public Guid PostId { get; set; }
    public Guid OwnerId { get; set; }
    public Guid TenantId { get; set; }

    // Thông tin bài đăng (từ RentalPostClientService)
    public RentalPostResponse PostInfo { get; set; }

    // Thông tin người dùng (từ AccountClientService)
    public AccountResponse Owner { get; set; }
    public AccountResponse Tenant { get; set; }

    // Danh sách tin nhắn trong phòng
    public List<ChatMessageResponse>? Messages { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? LastMessageAt { get; set; }
}