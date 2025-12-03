using ChatAPI.DTO.Request;
using Shared.DTOs.Amenities.Responses;
using Shared.DTOs;
using Shared.DTOs.Chats.Responses;

namespace ChatAPI.Service.Interface;
public interface IChatService
{ 
   Task<ApiResponse<ChatRoomResponse>> CreateChatRoom(Guid ownerId, Guid userId);
   Task<ApiResponse<bool>> Delete(Guid chatMessageId);
   Task<ApiResponse<List<ChatMessageResponse>>> GetByChatRoomId(Guid chatRoomId,  Guid senderId);
   Task<ApiResponse<List<ChatRoomResponse>>> GetAllChatRoomByOwner(Guid ownerId);
   Task<ApiResponse<List<ChatRoomResponse>>> GetAllChatRoomByUser(Guid userId);
   Task<ApiResponse<ChatMessageResponse>> SendMessage(Guid chatRoomId, Guid senderId,CreateChatMessage request);
}