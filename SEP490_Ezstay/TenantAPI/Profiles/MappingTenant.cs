using AutoMapper;
using TenantAPI.DTO.Requests;
using TenantAPI.Model;

namespace TenantAPI.Profiles;

public class MappingTenant:Profile
{
    public MappingTenant()
    {
        CreateMap<CreateTenantDto, Tenant>();
        CreateMap<UpdateTenantDto, Tenant>();
        CreateMap<Tenant, TenantDto>();
    }
}