



using ContractAPI.DTO.Requests;
using Shared.DTOs;
using Shared.DTOs.Contracts.Responses;
using Shared.Enums;

namespace ContractAPI.Services.Interfaces;
public interface IContractService
{
    IQueryable<ContractResponse> GetAllByTenantId(Guid tenantId);
    IQueryable<ContractResponse> GetAllByOwnerId(Guid ownerId);
    Task<ContractResponse?> GetByIdAsync(Guid id);
    Task<ApiResponse<ContractResponse>> Add(Guid ownerId, CreateContract request);
    Task<ApiResponse<bool>> UpdateAsync(Guid id, UpdateContract request);
    Task<ApiResponse<ContractResponse>> ExtendContract(Guid contractId, ExtendContract request);
    Task<ApiResponse<ContractResponse>> CancelContract(Guid contractId, string reason);
    Task<ApiResponse<bool>> Delete(Guid id);
    Task<ApiResponse<List<string>>> UploadContractImages(Guid id, IFormFileCollection images);
    Task<ApiResponse<bool>> ExistsByRoomId(Guid roomId);
    Task<ApiResponse<ContractResponse>> SignContract(Guid contractId, string ownerSignature, string role);

    Task<ApiResponse<RentalRequestResponse>> Add(Guid ownerId,Guid userId, Guid roomId, CreateRentalRequest request);
    IQueryable<RentalRequestResponse> GetAllRentalByUserId(Guid userId);
    IQueryable<RentalRequestResponse> GetAllRentalByOwnerId(Guid ownerId);

}
