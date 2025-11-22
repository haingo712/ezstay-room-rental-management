using AutoMapper;
using RoomAmenityAPI.DTO.Request;

using RoomAmenityAPI.Model;
using Shared.DTOs.RoomAmenities.Responses;

namespace RoomAmenityAPI.Profiles;

public class RoomAmenityProfile:Profile
{
    public RoomAmenityProfile()
    {
        CreateMap<CreateRoomAmenity, RoomAmenity>();
        CreateMap<List<CreateRoomAmenity>, RoomAmenity>();
        CreateMap<RoomAmenity, RoomAmenityResponse>();
        CreateMap<RoomAmenityResponse, RoomAmenityResponse>();

    }
}