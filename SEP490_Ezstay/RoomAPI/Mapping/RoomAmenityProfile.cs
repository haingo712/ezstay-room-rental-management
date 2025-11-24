using AutoMapper;
using RoomAPI.DTO.Request;
using RoomAPI.Model;
using Shared.DTOs.RoomAmenities.Responses;

namespace RoomAPI.Profiles;

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