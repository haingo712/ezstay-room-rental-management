using ChatAPI.DTO.Request;
using Shared.DTOs.Amenities.Responses;
using Shared.DTOs;
using Shared.DTOs.Chats.Responses;

namespace ChatAPI.Service.Interface;
public interface IChatService
{ 
   Task<ApiResponse<ChatRoomResponse>> Add(Guid ownerId, Guid userId);
  // Task<ApiResponse<ChatRoomResponse>> Update(Guid id, );
   Task<ApiResponse<bool>> Delete(Guid chatMessageId);
   Task<ApiResponse<List<ChatMessageResponse>>> GetByChatRoomId(Guid chatRoomId);
   Task<ApiResponse<List<ChatRoomResponse>>> GetAllChatRoom(Guid accountId);
   Task<ApiResponse<ChatMessageResponse>> SendMessage(Guid chatRoomId, Guid senderId,CreateChatMessage request);
}