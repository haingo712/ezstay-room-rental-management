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
            CreateMap<CreateSupportRequest, Support>();
            CreateMap<UpdateSupportStatusRequest, Support>();

            // Map model -> response
            CreateMap<Support, SupportResponse>();
        }
    }
}
