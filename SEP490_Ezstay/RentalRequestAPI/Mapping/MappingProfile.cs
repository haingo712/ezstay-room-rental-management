using AutoMapper;
using RentalRequestAPI.DTO.Request;
using RentalRequestAPI.Model;

namespace RentalRequestAPI.Mapping;

public class MappingProfile:Profile
{
    public MappingProfile()
    {
        CreateMap<CreateRentalRequestDto, RentalRequest>();
        CreateMap<UpdateRentalRequestDto, RentalRequest>();
        CreateMap<RentalRequest, RentalRequestDto>();
    }
}