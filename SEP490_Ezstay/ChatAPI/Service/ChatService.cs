using AutoMapper;
using ChatAPI.DTO.Request;
using ChatAPI.Models;
using ChatAPI.Repository.Interface;
using ChatAPI.Service.Interface;
using Shared.DTOs;
using Shared.DTOs.Chats.Responses;

namespace ChatAPI.Service;



public class ChatService(
    IChatRoomRepository _chatRoomRepository,
    IAuthService _authService,
    IChatMessageRepository _chatMessageRepository,
    IMapper _mapper,
    IImageService _imageService
    ): IChatService
{
    public async Task<ApiResponse<ChatRoomResponse>> Add(Guid ownerId, Guid userId)
    {
        var existing = await _chatRoomRepository.GetByOwnerAndUsers(ownerId, userId);
        if (existing != null)
            return ApiResponse<ChatRoomResponse>.Success(_mapper.Map<ChatRoomResponse>(existing), "");
        var chatRoom = new ChatRoom
        {
            OwnerId = ownerId,
            UserId = userId,
        };
        var result= await _chatRoomRepository.Add(chatRoom);
        return ApiResponse<ChatRoomResponse>.Success(   _mapper.Map<ChatRoomResponse>(result),"ok");
    }
    
    public async Task<ApiResponse<bool>> Delete(Guid id)
    {
        var room = await _chatMessageRepository.GetById(id);
        if (room==null) 
            throw new KeyNotFoundException("Not found");
        await _chatMessageRepository.Delete(room);
        return ApiResponse<bool>.Success(true, "Delete Successfully");
    }
    public async Task<ApiResponse<List<ChatMessageResponse>>> GetByChatRoomId(Guid chatRoomId)
    {
        var messages = await _chatMessageRepository.GetByChatRoomId(chatRoomId);
        return ApiResponse<List<ChatMessageResponse>>.Success( _mapper.Map<List<ChatMessageResponse>>(messages), "ok");
    }
    public async Task<ApiResponse<List<ChatRoomResponse>>> GetAllChatRoom(Guid accountId)
    {
      var chatRooms = await _chatRoomRepository.GetAllChatRoom(accountId);
      var tasks = chatRooms.Select(async room =>
      {
          var response = _mapper.Map<ChatRoomResponse>(room);

          var tenantTask = _authService.GetById(room.UserId);
          var ownerTask = _authService.GetById(room.OwnerId);
          await Task.WhenAll(tenantTask, ownerTask);
          response.User = tenantTask.Result;
          response.Owner = ownerTask.Result;
          return response;
      });
      var result = await Task.WhenAll(tasks);
      //  var result = new List<ChatRoomResponse>();
      // foreach (var r in chatRooms)
      // {
      //     // Console.WriteLine("sss "+await _accountClientService.GetByIdAsync(r.TenantId));
      //     // Console.WriteLine("sss "+ await _accountClientService.GetByIdAsync(r.OwnerId));
      //     var response = _mapper.Map<ChatRoomResponse>(r);
      //     response.User = await _accountClientService.GetByIdAsync(r.TenantId); 
      //     response.Owner = await _accountClientService.GetByIdAsync(r.OwnerId); 
      //     response.RentalPost = await _rentalPostClientService.GetById(r.PostId);
      //    
      //     result.Add(response);
      // } 
       return ApiResponse<List<ChatRoomResponse>>.Success( _mapper.Map<List<ChatRoomResponse>>(result), "ok");
    }

    public async Task<ApiResponse<ChatMessageResponse>> SendMessage(Guid chatRoomId, Guid senderId, CreateChatMessage request)
    {
        var chatMessage1 = await _chatMessageRepository.GetByChatRoomId(chatRoomId);
        if (chatMessage1 == null)
            throw new KeyNotFoundException("ChatRoom not found");
        var chatMessage = _mapper.Map<ChatMessage>(request);
        chatMessage.ChatRoomId = chatRoomId;
        chatMessage.SentAt = DateTime.UtcNow;
        chatMessage.SenderId = senderId;
        
        // Only upload images if there are any files
        if (request.Image != null)
        {
            chatMessage.Image = await _imageService.UploadMultipleImage(request.Image);
        }
        else
        {
            chatMessage.Image = new List<string>();
        }
        
        await _chatMessageRepository.Add(chatMessage);
        //   await _chatRoomRepository.UpdateLastMessageAt(chatRoomId, message.SentAt);
        return ApiResponse<ChatMessageResponse>.Success(_mapper.Map<ChatMessageResponse>(chatMessage),
            "Send Message Successfully");
    }
}