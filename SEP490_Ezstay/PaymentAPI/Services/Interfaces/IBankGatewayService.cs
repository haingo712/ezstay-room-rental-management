
using PaymentAPI.Model;
using Shared.DTOs;
using Shared.DTOs.Payments.Responses;

namespace PaymentAPI.Services.Interfaces;

public interface IBankGatewayService
{
 //   Task<int> SyncFromVietQR();
 Task<List<BankGatewayResponse>> SyncFromVietQR();
    //Task<List<BankGatewayResponse>> GetAllBankGateways();
    IQueryable<BankGatewayResponse> GetAllBankGateways();
    Task<ApiResponse<bool>> HiddenBankGateway(Guid id, bool isActive);
}