using ReviewAPI.DTO.Response;

namespace ReviewAPI.Service.Interface;

public interface IContractClientService
{
    Task<bool> CheckTenantHasContract(Guid tenantId,Guid roomId);
    Task<ContractResponseDto?> GetContractById(Guid contractId);
    // Task<bool> CheckTenantHasContract(Guid tenantId,Guid roomId);
}