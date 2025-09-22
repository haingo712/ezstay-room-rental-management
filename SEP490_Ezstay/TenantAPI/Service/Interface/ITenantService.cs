
using TenantAPI.DTO.Requests;
using TenantAPI.DTO.Response;

namespace TenantAPI.Service.Interface;

public interface ITenantService
{
    IQueryable<TenantDto> GetAllQueryable();
    IQueryable<TenantDto> GetAllByUserId(Guid userId);
    IQueryable<TenantDto> GetAllByOwnerId(Guid ownerId);
  //  IQueryable<TenantDto> GetAllByRoomId(Guid roomId);
    Task<TenantDto?> GetByIdAsync(Guid id);
    Task<ApiResponse<TenantDto>> AddAsync(Guid ownerId ,CreateTenantDto request);
    Task<ApiResponse<TenantDto>> UpdateAsync(Guid id, UpdateTenantDto request);
    Task<ApiResponse<TenantDto>> ExtendContractAsync(Guid tenantId, ExtendTenantDto request);
    Task<ApiResponse<TenantDto>> CancelTenantAsync(Guid tenantId, string reason);

}