using AutoMapper;
using ContractAPI.DTO.Requests;
using ContractAPI.DTO.Response;
using ContractAPI.Model;

namespace ContractAPI.Profiles;

public class MappingIdentityProfile: Profile
{
    public MappingIdentityProfile()
    {
        CreateMap<CreateIdentityProfileDto, IdentityProfile>();
        CreateMap<UpdateIdentityProfileDto, IdentityProfile>();
        CreateMap<IdentityProfile, IdentityProfileResponseDto>();
    }
}