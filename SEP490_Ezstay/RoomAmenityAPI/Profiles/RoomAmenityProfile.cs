using AutoMapper;
using RoomAmenityAPI.DTO.Request;

using RoomAmenityAPI.Model;
using RoomAmenityAPI.Grpc;
using Shared.DTOs.RoomAmenities.Responses;

namespace RoomAmenityAPI.Profiles;

public class RoomAmenityProfile:Profile
{
    public RoomAmenityProfile()
    {
        CreateMap<CreateRoomAmenity, RoomAmenity>();
        CreateMap<List<CreateRoomAmenity>, RoomAmenity>();
        CreateMap<RoomAmenity, RoomAmenityResponse>();
        CreateMap<RoomAmenityResponse, GetRoomAmenityResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
            .ForMember(dest => dest.RoomId, opt => opt.MapFrom(src => src.RoomId.ToString()))
            .ForMember(dest => dest.AmenityId, opt => opt.MapFrom(src => src.AmenityId.ToString()));
    
    }
}