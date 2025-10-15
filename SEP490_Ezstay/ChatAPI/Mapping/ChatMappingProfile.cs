using AutoMapper;
using ChatAPI.DTO.Request;
using ChatAPI.Models;
using Shared.DTOs.Chats.Responses;

namespace ChatAPI.Mapping;

public class ChatMappingProfile:Profile
{
    public ChatMappingProfile()
    {
        CreateMap<CreateChatRoom, ChatRoom>();
        // CreateMap<UpdateAmenity, ChatRoom>();
        CreateMap<CreateChatMessage, ChatMessage>();

        CreateMap<ChatRoom, ChatRoomResponse>();
        CreateMap<ChatMessage, ChatMessageResponse>();

    }
}