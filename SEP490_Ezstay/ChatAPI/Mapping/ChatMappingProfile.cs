using AutoMapper;
using ChatAPI.DTO.Request;
using ChatAPI.Models;
using Shared.DTOs.Chats.Responses;

namespace ChatAPI.Mapping;

public class ChatMappingProfile:Profile
{
    public ChatMappingProfile()
    {
        CreateMap<CreateChatMessage, ChatMessage>();
        CreateMap<ChatRoom, ChatRoomResponse>();
        CreateMap<ChatMessage, ChatMessageResponse>();

    }
}