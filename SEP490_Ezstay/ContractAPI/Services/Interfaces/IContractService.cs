



using ContractAPI.DTO.Requests;
using ContractAPI.DTO.Response;
using ContractAPI.Enum;

namespace ContractAPI.Services.Interfaces;
public interface IContractService
{
    IQueryable<ContractResponseDto> GetAllQueryable();
    IQueryable<ContractResponseDto> GetAllByTenantId(Guid tenantId);
    IQueryable<ContractResponseDto> GetAllByOwnerId(Guid ownerId);
    IQueryable<ContractResponseDto> GetAllByOwnerId(Guid ownerId, ContractStatus contractStatus);
    Task<ContractResponseDto?> GetByIdAsync(Guid id);
  //  Task<ApiResponse<ContractResponseDto>> AddAsync(Guid ownerId, CreateContractDto request);
    Task<ApiResponse<ContractResponseDto>> Add(Guid ownerId, CreateContractDto request);
    Task<ApiResponse<ContractResponseDto>> UpdateAsync(Guid id, UpdateContractDto request);
    Task<ApiResponse<ContractResponseDto>> ExtendContractAsync(Guid contractId, ExtendContractDto request);
    Task<ApiResponse<ContractResponseDto>> CancelContractAsync(Guid contractId, string reason);
    Task<ApiResponse<bool>> DeleteAsync(Guid id);
}
