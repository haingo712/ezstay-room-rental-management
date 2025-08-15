using AutoMapper;
using AmenityAPI.DTO.Request;
using AmenityAPI.Models;

namespace AmenityAPI.Mapping;

public class MappingAmenity:Profile
{
    public MappingAmenity()
    {
        CreateMap<CreateAmenityDto, Amenity>();
        CreateMap<UpdateAmenityDto, Amenity>();
        CreateMap<Amenity, AmenityDto>();
    }
}