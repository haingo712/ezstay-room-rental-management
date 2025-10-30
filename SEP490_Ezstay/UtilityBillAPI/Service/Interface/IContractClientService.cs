using Shared.DTOs.Contracts.Responses;

namespace UtilityBillAPI.Service.Interface
{
    public interface IContractClientService
    {
        Task<ContractResponse?> GetContractAsync(Guid contractId);
    }
}
