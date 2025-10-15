



using ContractAPI.DTO.Requests;
using Shared.DTOs;
using Shared.DTOs.Contracts.Responses;
using Shared.Enums;

namespace ContractAPI.Services.Interfaces;
public interface IContractService
{
    IQueryable<ContractResponse> GetAllQueryable();
   // Task<bool> HasContractAsync(Guid tenantId, Guid roomId);
   IQueryable<ContractResponse> GetAllByTenantId(Guid tenantId);
    IQueryable<ContractResponse> GetAllByOwnerId(Guid ownerId);
    IQueryable<ContractResponse> GetAllByOwnerId(Guid ownerId, ContractStatus contractStatus);
    Task<ContractResponse?> GetByIdAsync(Guid id);
    Task<ApiResponse<ContractResponse>> Add(Guid ownerId, CreateContract request);
    Task<ApiResponse<bool>> UpdateAsync(Guid id, UpdateContract request);
    Task<ApiResponse<ContractResponse>> ExtendContract(Guid contractId, ExtendContractDto request);
    Task<ApiResponse<ContractResponse>> CancelContract(Guid contractId, string reason);
    Task<ApiResponse<bool>> Delete(Guid id);
    Task<ApiResponse<List<string>>> UploadContractImages(Guid id, List<IFormFile> images);
}
