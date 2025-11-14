using AutoMapper;
using ServiceAPI.DTO.Response;
using ServiceAPI.DTO.Resquest;
using ServiceAPI.Model;

namespace ServiceAPI.Mapping
{
    public class ServiceProfile : Profile
    {
        public ServiceProfile()
        {
            CreateMap<ServiceRequestDto, ServiceModel>();
            CreateMap<ServiceModel, ServiceResponseDto>();
        }
    }
}
