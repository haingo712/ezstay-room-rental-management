



using ContractAPI.DTO.Requests;
using Shared.DTOs;
using Shared.DTOs.Contracts.Responses;
using Shared.Enums;

namespace ContractAPI.Services.Interfaces;
public interface IContractService
{
    
    Task<ApiResponse<ContractResponse>> SignContract(Guid contractId, string ownerSignature,  string role);
    IQueryable<ContractResponse> GetAllByTenantId(Guid userId);
    IQueryable<ContractResponse> GetAllByOwnerId(Guid ownerId);
    Task<ContractResponse?> GetByIdAsync(Guid id);
    Task<ApiResponse<ContractResponse>> Add(Guid ownerId, CreateContract request);
    Task<ApiResponse<bool>> UpdateAsync(Guid id, UpdateContract request);
    Task<ApiResponse<ContractResponse>> ExtendContract(Guid contractId, ExtendContract request);
    Task<ApiResponse<ContractResponse>> CancelContract(Guid contractId, string reason);
    Task<ApiResponse<bool>> Delete(Guid id);
    Task<ApiResponse<List<string>>> UploadContractImages(Guid id, IFormFileCollection images);
    Task<ApiResponse<bool>> ExistsByRoomId(Guid roomId);
    Task<ApiResponse<ContractResponse>> SignContractOwner(Guid contractId, string ownerSignature, Guid ownerId );
    Task<ApiResponse<ContractResponse>> SignContractUser(Guid contractId, string ownerSignature,  Guid userId);
    //Task<ApiResponse<ContractResponse>> SignContract(Guid contractId, string ownerSignature,  string role);

    Task<ApiResponse<RentalRequestResponse>> Add(Guid ownerId,Guid userId, Guid roomId, CreateRentalRequest request);
    IQueryable<RentalRequestResponse> GetAllRentalByUserId(Guid userId);
    IQueryable<RentalRequestResponse> GetAllRentalByOwnerId(Guid ownerId);
    
    // Async versions with user/owner info populated
    Task<List<RentalRequestResponse>> GetAllRentalByOwnerIdWithUserInfoAsync(Guid ownerId);
    Task<List<RentalRequestResponse>> GetAllRentalByUserIdWithOwnerInfoAsync(Guid userId);
    
    // Get all tenants for owner
    Task<List<TenantInfoResponse>> GetAllTenantsAsync(Guid ownerId);

}
