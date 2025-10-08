using AutoMapper;
using ContractAPI.DTO.Requests;
using ContractAPI.DTO.Response;
using ContractAPI.Model;

namespace ContractAPI.Profiles;

public class MappingIdentityProfile: Profile
{
    public MappingIdentityProfile()
    {
        CreateMap<CreateIdentityProfile, IdentityProfile>();
        CreateMap<UpdateIdentityProfile, IdentityProfile>();
        CreateMap<IdentityProfile, IdentityProfileResponseDto>();
    }
}