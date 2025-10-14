using System.Security.Claims;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ChatAPI.DTO.Request;
using ChatAPI.Models;
using ChatAPI.Repository.Interface;
using ChatAPI.Service.Interface;
using Shared.DTOs;
using Shared.DTOs.Chats.Responses;

namespace ChatAPI.Service;



public class ChatService: IChatService
{
    // private readonly IMapper _mapper;
    // private readonly IChatRepository _chatRepository;
    // private readonly IImageAPI _imageClient;
    // private readonly IRoomAmenityAPI _roomAmenityAPI;
    // public ChatService(IMapper mapper, IChatRepository chatRepository, IImageAPI imageClient, IRoomAmenityAPI roomAmenityAPI)
    // {
    //     _mapper = mapper;
    //     _chatRepository = chatRepository;
    //     _imageClient = imageClient;
    //     _roomAmenityAPI = roomAmenityAPI;
    // }
    private readonly IChatRoomRepository _chatRoomRepository;
    private readonly IAccountClientService _accountClientService;
    private readonly IChatMessageRepository _chatMessageRepository;
  private readonly IRentalPostClientService _rentalPostClientService;
    private readonly IMapper _mapper;

    public ChatService(IChatRoomRepository chatRoomRepository, IAccountClientService accountClientService, IChatMessageRepository chatMessageRepository, IRentalPostClientService rentalPostClientService, IMapper mapper)
    {
        _chatRoomRepository = chatRoomRepository;
        _accountClientService = accountClientService;
        _chatMessageRepository = chatMessageRepository;
        _rentalPostClientService = rentalPostClientService;
        _mapper = mapper;
    }
    public async Task<ApiResponse<ChatRoomResponse>> Add(Guid postId, Guid userId)
    {
        var existing = await _chatRoomRepository.GetByPostAndUsers(postId, userId);
        if (existing != null)
            return ApiResponse<ChatRoomResponse>.Success(_mapper.Map<ChatRoomResponse>(existing),"ok");
      var post = _rentalPostClientService.GetByIdAsync(postId);
        var chatRoom = new ChatRoom
        {
            PostId = postId,
           OwnerId = post.Result.AuthorId,
            TenantId = userId,
            CreatedAt = DateTime.UtcNow,
            LastMessageAt = DateTime.UtcNow
        };
        var result= await _chatRoomRepository.Add(chatRoom);
        
        return ApiResponse<ChatRoomResponse>.Success(_mapper.Map<ChatRoomResponse>(result),"ok");
    }
    public async Task<ApiResponse<List<ChatMessageResponse>>> GetMessages(Guid chatRoomId)
    {
        var messages = await _chatMessageRepository.GetByChatRoomId(chatRoomId);
        return ApiResponse<List<ChatMessageResponse>>.Success( _mapper.Map<List<ChatMessageResponse>>(messages), "ok");
    }
    public async Task<ApiResponse<List<ChatRoomResponse>>> GetAllChatRoomByOwner(Guid ownerId)
    {
      var chatRooms = await _chatRoomRepository.GetByChatRoomByOwner(ownerId);
      
      var result = new List<ChatRoomResponse>();
    
      foreach (var r in chatRooms)
      {
          Console.WriteLine("sss "+await _accountClientService.GetByIdAsync(r.TenantId));
          Console.WriteLine("sss "+ await _accountClientService.GetByIdAsync(r.OwnerId));
          var response = _mapper.Map<ChatRoomResponse>(r);
          var tenantInfo = await _accountClientService.GetByIdAsync(r.TenantId);
          response.user = tenantInfo; 
          var ownerInfor = await _accountClientService.GetByIdAsync(r.OwnerId);
          response.owner = ownerInfor; 
          result.Add(response);
      } 
       return ApiResponse<List<ChatRoomResponse>>.Success( _mapper.Map<List<ChatRoomResponse>>(result), "ok");
    }
    public async Task<ApiResponse<ChatMessageResponse>> SendMessage(Guid chatRoomId,Guid senderId,CreateChatMessage request)
    {
        var chatMessage = _chatMessageRepository.GetByChatRoomId(chatRoomId);
        if(chatMessage == null)
            throw new KeyNotFoundException("ChatRoom not found");
        var messageDto = _mapper.Map<ChatMessage>(request);
        messageDto.ChatRoomId = chatRoomId;
        messageDto.SentAt = DateTime.UtcNow;
        messageDto.SenderId = senderId;
        await _chatMessageRepository.Add(messageDto);
        //   await _chatRoomRepository.UpdateLastMessageAt(chatRoomId, message.SentAt);
        return ApiResponse<ChatMessageResponse>.Success(_mapper.Map<ChatMessageResponse>(messageDto), "Guiwr message thanh cong");
    }
    // // Gửi tin nhắn
    // public async Task<ApiResponse<ChatMessageResponse>> SendMessage(Guid chatRoomId,CreateChatMessage request)
    // {
    //     var chatMessage = _chatMessageRepository.GetByChatRoomId(chatRoomId);
    //     if(chatMessage == null)
    //         throw new KeyNotFoundException("ChatRoom not found");
    //     var messageDto = _mapper.Map<ChatMessage>(request);
    //     messageDto.ChatRoomId = chatRoomId;
    //     messageDto.SentAt = DateTime.UtcNow;
    //      await _chatMessageRepository.Add(messageDto);
    //  //   await _chatRoomRepository.UpdateLastMessageAt(chatRoomId, message.SentAt);
    //     return ApiResponse<ChatMessageResponse>.Success(_mapper.Map<ChatMessageResponse>(messageDto), "ok");
    // }
    
    public async Task<ApiResponse<List<ChatRoomResponse>>> GetChatRoomsByOwner(Guid ownerId)
    {
        var rooms = await _chatRoomRepository.GetByOwner(ownerId);
        return ApiResponse<List<ChatRoomResponse>>.Success( _mapper.Map<List<ChatRoomResponse>>(rooms),"ok");
    }
    
    public async Task<ApiResponse<List<ChatRoomResponse>>> GetChatRoomsByTenant(Guid tenantId)
    {
        var rooms = await _chatRoomRepository.GetByTenant(tenantId);
        return ApiResponse<List<ChatRoomResponse>>.Success( _mapper.Map<List<ChatRoomResponse>>(rooms),"ok");
    }
}