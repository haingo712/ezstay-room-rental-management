using UtilityRateAPI.DTO.Request;
using AutoMapper;
using UtilityRateAPI.Model;

namespace UtilityRateAPI.Mapping;

public class MappingProfile:Profile
{
    public MappingProfile()
    {
        CreateMap<CreateUtilityRateDto, UtilityRate>();
        CreateMap<UpdateUtilityRateDto, UtilityRate>();
        CreateMap<UtilityRate, UtilityRateDto>();
    }
}