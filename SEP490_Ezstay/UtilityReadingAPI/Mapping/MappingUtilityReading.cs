using AutoMapper;
using Shared.DTOs.UtilityReadings.Responses;
using UtilityReadingAPI.DTO.Request;
using UtilityReadingAPI.Model;

namespace UtilityReadingAPI.Mapping;

public class MappingUtilityReading:Profile
{
    public MappingUtilityReading()
    {
        CreateMap<CreateUtilityReading , UtilityReading>();
        CreateMap<CreateUtilityReadingContract , UtilityReading>();
        CreateMap<UpdateUtilityReading, UtilityReading>();
        CreateMap<UtilityReading,  UtilityReadingResponse>();
    }
}
