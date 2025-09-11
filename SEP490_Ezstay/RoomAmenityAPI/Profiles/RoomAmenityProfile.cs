using AutoMapper;
using RoomAmenityAPI.DTO.Request;
using RoomAmenityAPI.Model;
using RoomAmenityAPI.Grpc;
namespace RoomAmenityAPI.Profiles;

public class RoomAmenityProfile:Profile
{
    public RoomAmenityProfile()
    {
        CreateMap<CreateRoomAmenityDto, RoomAmenity>();
        CreateMap<UpdateRoomAmenityDto, RoomAmenity>();
        CreateMap<RoomAmenity, RoomAmenityDto>();
        CreateMap<RoomAmenityDto, GetRoomAmenityResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
            .ForMember(dest => dest.RoomId, opt => opt.MapFrom(src => src.RoomId.ToString()))
            .ForMember(dest => dest.AmenityId, opt => opt.MapFrom(src => src.AmenityId.ToString()));
    
    }
}