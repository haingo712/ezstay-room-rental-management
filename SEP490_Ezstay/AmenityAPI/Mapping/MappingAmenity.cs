using AutoMapper;
using AmenityAPI.DTO.Request;
using AmenityAPI.Models;
using Shared.DTOs.Amenities.Responses;

namespace AmenityAPI.Mapping;

public class MappingAmenity:Profile
{
    public MappingAmenity()
    {
        CreateMap<CreateAmenity, Amenity>();
        CreateMap<UpdateAmenity, Amenity>();
        CreateMap<Amenity, AmenityResponse>();
    }
}