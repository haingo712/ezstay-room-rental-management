using AutoMapper;
using ContractAPI.DTO.Requests;
using ContractAPI.DTO.Response;
using ContractAPI.Model;
using Shared.DTOs.Contracts.Responses;
using IdentityProfileResponse = Shared.DTOs.Contracts.Responses.IdentityProfileResponse;

namespace ContractAPI.Profiles;

public class MappingIdentityProfile: Profile
{
    public MappingIdentityProfile()
    {
        CreateMap<CreateIdentityProfile, IdentityProfile>();
        CreateMap<UpdateIdentityProfile, IdentityProfile>();
        CreateMap<IdentityProfile, IdentityProfileResponse>();
    }
}