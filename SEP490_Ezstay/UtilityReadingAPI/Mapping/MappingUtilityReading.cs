using AutoMapper;
using UtilityReadingAPI.DTO.Response;
using UtilityReadingAPI.DTO.Request;
using UtilityReadingAPI.Model;

namespace UtilityReadingAPI.Mapping;

public class MappingUtilityReading:Profile
{
    public MappingUtilityReading()
    {
        CreateMap<CreateUtilityReadingDto , UtilityReading>();
        CreateMap<CreateUtilityReadingContract , UtilityReading>();
        CreateMap<UpdateUtilityReadingDto, UtilityReading>();
        CreateMap<UtilityReading,  UtilityReadingResponseDto>();
    }
}
