using ReviewAPI.DTO.Response;

namespace ReviewAPI.Service.Interface;

public interface IContractService
{
  //  Task<bool> CheckTenantHasContract(Guid tenantId,Guid roomId);
    Task<ContractResponse> GetContractId(Guid contractId);
    // Task<bool> CheckTenantHasContract(Guid tenantId,Guid roomId);
}