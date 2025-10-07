using IdentityProfileAPI.DTO.Requests;
using IdentityProfileAPI.DTO.Response;

namespace IdentityProfileAPI.Services.Interfaces;

public interface IIdentityProfileService
{
    public IQueryable<IdentityProfileResponseDto> GetAllByTenantId(Guid tenantId);
    // Task<List<IdentityProfileResponseDto>> GetAllByTenantId(Guid tenantId);
    IQueryable<IdentityProfileResponseDto> GetAllQueryable();
    Task<IdentityProfileResponseDto?> GetByIdAsync(Guid id);
    // Task<ApiResponse<IdentityProfileResponseDto>> AddAsync(CreateIdentityProfileDto request);
    Task<ApiResponse<IdentityProfileResponseDto>> AddAsync(Guid contractId, CreateIdentityProfileDto request);
   // Task<ApiResponse<IdentityProfileResponseDto>> AddManyAsync(Guid contractId, List<CreateIdentityProfileDto> request);
    Task<ApiResponse<IdentityProfileResponseDto>> UpdateAsync(Guid id, UpdateIdentityProfileDto request);
    Task DeleteAsync(Guid id);   
}