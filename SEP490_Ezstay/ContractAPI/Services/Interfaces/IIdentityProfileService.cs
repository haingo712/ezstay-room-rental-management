using ContractAPI.DTO.Requests;
using ContractAPI.DTO.Response;

namespace ContractAPI.Services.Interfaces;

public interface IIdentityProfileService
{
    public IQueryable<IdentityProfileResponseDto> GetAllByTenantId(Guid tenantId);
    // Task<List<IdentityProfileResponseDto>> GetAllByTenantId(Guid tenantId);
    IQueryable<IdentityProfileResponseDto> GetAllQueryable();
    Task<IdentityProfileResponseDto?> GetByIdAsync(Guid id);
    // Task<ApiResponse<IdentityProfileResponseDto>> AddAsync(CreateIdentityProfileDto request);
    Task<ApiResponse<IdentityProfileResponseDto>> AddAsync(Guid contractId, CreateIdentityProfileDto request);
    Task<ApiResponse<IdentityProfileResponseDto>> UpdateAsync(Guid id, UpdateIdentityProfileDto request);
    Task DeleteAsync(Guid id);   
}