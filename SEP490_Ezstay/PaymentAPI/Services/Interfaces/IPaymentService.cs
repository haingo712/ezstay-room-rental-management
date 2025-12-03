using PaymentAPI.DTOs.Requests;
using Shared.DTOs;
using Shared.Enums;

namespace PaymentAPI.Services.Interfaces;

public interface IPaymentService
{
    Task<ApiResponse<bool>> HandleSePayWebhookAsync(CreatePayment request);
}
