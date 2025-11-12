using PaymentAPI.DTOs.Responses;

using PaymentAPI.DTOs.Responses;

namespace PaymentAPI.Services.Interfaces;

public interface ISePayService
{
    Task<SePayTransactionResponse> GetTransactionDetailsAsync(string transactionId);
    Task<bool> VerifyTransactionAsync(string transactionId, decimal expectedAmount, string expectedContent, string accountNumber);
    
    // New methods for polling
    Task<List<SePayTransactionDto>> GetRecentTransactionsAsync(string accountNumber, DateTime fromDate);
    Task<SePayTransactionDto?> FindTransactionByContentAsync(string accountNumber, string expectedContent, DateTime fromDate);
    Task<bool> CheckPaymentSuccessAsync(string billCode, decimal expectedAmount, string accountNumber);
}
