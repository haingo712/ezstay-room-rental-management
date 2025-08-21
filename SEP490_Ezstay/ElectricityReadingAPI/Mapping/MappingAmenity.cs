using AutoMapper;
using ElectricityReadingAPI.DTO.Request;
using ElectricityReadingAPI.Model;

namespace ElectricityReadingAPI.Mapping;

public class MappingAmenity:Profile
{
    public MappingAmenity()
    {
        CreateMap<CreateElectricityReadingDto , ElectricityReading>();
        CreateMap<UpdateElectricityReadingDto, ElectricityReading>();
        CreateMap<ElectricityReading,  ElectricityReadingDto>();
    }
}