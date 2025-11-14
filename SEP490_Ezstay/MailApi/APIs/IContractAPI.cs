using Shared.DTOs.Contracts.Responses;

namespace MailApi.APIs;

public interface IContractAPI
{
    Task<ContractResponse?> GetContractByIdAsync(Guid contractId);
}
