using AutoMapper;
using RoomAPI.DTO.Request;
using RoomAPI.Model;

namespace RoomAPI.Mapping;

public class MappingRoom:Profile
{
    public MappingRoom()
    {
        CreateMap<CreateRoomDto, Room>();
        CreateMap<UpdateRoomDto, Room>();
        CreateMap<Room, RoomDto>();
    }
}