using AutoMapper;
using RoomAmenityAPI.DTO.Request;
using RoomAmenityAPI.Model;

namespace RoomAmenityAPI.Mapping;

public class MappingRoomAmenity:Profile
{
    public MappingRoomAmenity()
    {
        CreateMap<CreateRoomAmenityDto, RoomAmenity>();
        CreateMap<UpdateRoomAmenityDto, RoomAmenity>();
        CreateMap<RoomAmenity, RoomAmenityDto>();
    }
}