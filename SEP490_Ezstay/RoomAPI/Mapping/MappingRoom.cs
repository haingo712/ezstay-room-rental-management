using AutoMapper;
using RoomAPI.DTO.Request;
using RoomAPI.Model;
using Shared.DTOs.Rooms.Responses;

namespace RoomAPI.Mapping;

public class MappingRoom:Profile
{
    public MappingRoom()
    {
        CreateMap<CreateRoom, Room>();
        CreateMap<UpdateRoom, Room>();
        CreateMap<Room, RoomResponse>();

        // CreateMap<Room, RoomWithAmenitiesDto>()
        //     .ForMember(dest => dest.Amenities,
        //         opt => opt.Ignore());
    }
}