using AutoMapper;
using TenantAPI.DTO.Request;
using TenantAPI.Model;

namespace TenantAPI.Mapping;

public class MappingTenant:Profile
{
    public MappingTenant()
    {
        CreateMap<CreateTenantDto, Tenant>();
        CreateMap<UpdateTenantDto, Tenant>();
        CreateMap<Tenant, TenantDto>();
    }
}