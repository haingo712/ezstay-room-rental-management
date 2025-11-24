
using PaymentAPI.Model;
using Shared.DTOs;
using Shared.DTOs.Payments.Responses;

namespace PaymentAPI.Services.Interfaces;

public interface IBankGatewayService
{
    Task<List<BankGatewayResponse>> SyncFromVietQR();
    IQueryable<BankGatewayResponse> GetAllBankGateway();
    IQueryable<BankGatewayResponse> GetAllActiveBankGateway();

    Task<ApiResponse<bool>> HiddenBankGateway(Guid id, bool isActive);
}