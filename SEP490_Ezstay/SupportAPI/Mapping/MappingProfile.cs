using AutoMapper;
using SupportAPI.DTO.Request;
using SupportAPI.DTO.Response;
using SupportAPI.Model;

namespace SupportAPI.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateSupportRequest, SupportModel>();
            CreateMap<UpdateSupportStatusRequest, SupportModel>();

            // Map model -> response
            CreateMap<SupportModel, SupportResponse>();
        }
    }
}
