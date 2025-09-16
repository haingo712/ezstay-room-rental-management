using AutoMapper;
using RoomAPI.DTO.Request;
using RoomAPI.DTO.Response;
using RoomAPI.Model;

namespace RoomAPI.Mapping;

public class MappingRoom:Profile
{
    public MappingRoom()
    {
        CreateMap<CreateRoomDto, Room>();
        CreateMap<UpdateRoomDto, Room>();
        CreateMap<Room, RoomDto>();

        CreateMap<Room, RoomWithAmenitiesDto>()
            .ForMember(dest => dest.Amenities,
                opt => opt.Ignore());
    }
}