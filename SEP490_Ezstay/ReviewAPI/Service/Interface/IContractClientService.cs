namespace ReviewAPI.Service.Interface;

public interface IContractClientService
{
    Task<bool> CheckTenantHasContract(Guid tenantId,Guid roomId);
}