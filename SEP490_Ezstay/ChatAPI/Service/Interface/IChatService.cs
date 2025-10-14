using ChatAPI.DTO.Request;
using Shared.DTOs.Amenities.Responses;
using Shared.DTOs;
using Shared.DTOs.Chats.Responses;

namespace ChatAPI.Service.Interface;



public interface IChatService
{
  //  Task<ApiResponse<IEnumerable<AmenityResponseDto>>>  GetAllByStaffId(Guid staffId);
  //  IQueryable<AmenityResponseDto> GetAllByStaffIdAsQueryable(Guid staffId);
   //  IQueryable<AmenityResponse> GetAllAsQueryable();
   //  Task<AmenityResponse> GetById(Guid id);
   // Task<IEnumerable<ChatResponse>> GetRoomsAsync(Guid userId);
   // Task<IEnumerable<ChatResponse>> GetMessagesAsync(Guid roomId);
   // Task SendMessageAsync(Guid roomId, Guid senderId, string content);
   // Task<ApiResponse<bool>> Add(Guid ownerId, Guid tenantId, Guid? contractId);
   //  // Task<ApiResponse<bool>> Update(Guid id,UpdateAmenity request);
   //  Task<ApiResponse<bool>> Delete(Guid id);
   // Task<ChatRoomResponse> CreateOrGetRoomAsync(ChatRoomRequest request);
   // Task<List<ChatRoomResponse>> GetUserRoomsAsync(Guid userId);
   // Task<List<ChatMessageResponse>> GetMessagesAsync(Guid roomId);
   // Task<ChatMessageResponse> SendMessageAsync(ChatMessageRequest request);

   // Task<ApiResponse<ChatRoomResponse>> Add(Guid postId, CreateChatRoom request);

   Task<ApiResponse<ChatRoomResponse>> Add(Guid postId, Guid userId);
   Task<ApiResponse<List<ChatMessageResponse>>> GetMessages(Guid chatRoomId);
   Task<ApiResponse<List<ChatRoomResponse>>> GetAllChatRoomByOwner(Guid ownerId);
   Task<ApiResponse<ChatMessageResponse>> SendMessage(Guid chatRoomId, Guid senderId,CreateChatMessage request);
   // Task<ApiResponse<ChatMessageResponse>> SendMessage(Guid chatRoomId,CreateChatMessage request);
   Task<ApiResponse<List<ChatRoomResponse>>> GetChatRoomsByOwner(Guid ownerId);
   Task<ApiResponse<List<ChatRoomResponse>>> GetChatRoomsByTenant(Guid tenantId);

}