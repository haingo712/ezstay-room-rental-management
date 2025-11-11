using ReviewAPI.DTO.Response;
using Shared.DTOs.Contracts.Responses;

namespace ReviewAPI.Service.Interface;

public interface IContractClientService
{
  //  Task<bool> CheckTenantHasContract(Guid tenantId,Guid roomId);
    Task<ContractResponse?> GetContractId(Guid contractId);
    // Task<bool> CheckTenantHasContract(Guid tenantId,Guid roomId);
}