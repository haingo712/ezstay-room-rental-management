using Microsoft.Extensions.Options;
using PaymentAPI.Config;
using PaymentAPI.DTOs.Responses;
using PaymentAPI.Services.Interfaces;
using System.Text.Json;

namespace PaymentAPI.Services;

public class SePayService : ISePayService
{
    private readonly HttpClient _httpClient;
    private readonly SePayConfig _config;
    private readonly ILogger<SePayService> _logger;

    public SePayService(
        HttpClient httpClient,
        IOptions<SePayConfig> config,
        ILogger<SePayService> logger)
    {
        _httpClient = httpClient;
        _config = config.Value;
        _logger = logger;
        
        _httpClient.BaseAddress = new Uri(_config.ApiUrl);
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_config.SecretKey}");
    }

    public async Task<SePayTransactionResponse> GetTransactionDetailsAsync(string transactionId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/transactions/details/{transactionId}");
            
            var content = await response.Content.ReadAsStringAsync();
            _logger.LogInformation($"SePay API Response: {content}");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"SePay API Error: Status={response.StatusCode}, Content={content}");
                return new SePayTransactionResponse
                {
                    Status = false,
                    Message = $"Không thể lấy thông tin giao dịch: {response.StatusCode}"
                };
            }

            var result = JsonSerializer.Deserialize<SePayTransactionResponse>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result ?? new SePayTransactionResponse 
            { 
                Status = false, 
                Message = "Không thể parse response từ SePay" 
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error calling SePay API for transaction {transactionId}");
            return new SePayTransactionResponse
            {
                Status = false,
                Message = $"Lỗi kết nối SePay: {ex.Message}"
            };
        }
    }

    public async Task<bool> VerifyTransactionAsync(string transactionId, decimal expectedAmount, string expectedContent, string accountNumber)
    {
        try
        {
            var response = await GetTransactionDetailsAsync(transactionId);
            
            if (!response.Status || response.Data == null)
            {
                _logger.LogWarning($"Transaction {transactionId} not found or failed");
                return false;
            }

            var transaction = response.Data;
            
            // Verify account number (số tài khoản nhận tiền)
            if (transaction.AccountNumber != accountNumber)
            {
                _logger.LogWarning($"Account number mismatch. Expected: {accountNumber}, Got: {transaction.AccountNumber}");
                return false;
            }

            // Verify amount
            if (transaction.Amount != expectedAmount)
            {
                _logger.LogWarning($"Amount mismatch. Expected: {expectedAmount}, Got: {transaction.Amount}");
                return false;
            }

            // Verify content contains expected code
            if (!transaction.Description.Contains(expectedContent, StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogWarning($"Content mismatch. Expected to contain: {expectedContent}, Got: {transaction.Description}");
                return false;
            }

            _logger.LogInformation($"Transaction {transactionId} verified successfully");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error verifying transaction {transactionId}");
            return false;
        }
    }

    public async Task<List<SePayTransactionDto>> GetRecentTransactionsAsync(string accountNumber, DateTime fromDate)
    {
        try
        {
            var dateStr = fromDate.ToString("yyyy-MM-dd HH:mm:ss");
            var response = await _httpClient.GetAsync(
                $"/userapi/transactions/list" +
                $"?account_number={accountNumber}" +
                $"&transaction_date_min={Uri.EscapeDataString(dateStr)}" +
                $"&limit=50"
            );
            
            var content = await response.Content.ReadAsStringAsync();
            _logger.LogInformation($"SePay Transactions Response: {content}");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Failed to get transactions: Status={response.StatusCode}, Content={content}");
                return new List<SePayTransactionDto>();
            }
            
            var result = JsonSerializer.Deserialize<SePayTransactionListResponse>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            
            return result?.Transactions ?? new List<SePayTransactionDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting recent transactions");
            return new List<SePayTransactionDto>();
        }
    }

    public async Task<SePayTransactionDto?> FindTransactionByContentAsync(
        string accountNumber, 
        string expectedContent, 
        DateTime fromDate)
    {
        try
        {
            var transactions = await GetRecentTransactionsAsync(accountNumber, fromDate);
            
            // Tìm giao dịch có nội dung chứa expected content
            var matchedTx = transactions.FirstOrDefault(tx => 
                tx.TransactionContent.Contains(expectedContent, StringComparison.OrdinalIgnoreCase) &&
                tx.AmountInDecimal > 0 // Chỉ lấy giao dịch tiền vào
            );
            
            if (matchedTx != null)
            {
                _logger.LogInformation($"Found matching transaction: {matchedTx.Id} - Amount: {matchedTx.AmountInDecimal}");
            }
            else
            {
                _logger.LogWarning($"No matching transaction found for content: {expectedContent}");
            }
            
            return matchedTx;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error finding transaction by content");
            return null;
        }
    }

    public async Task<bool> CheckPaymentSuccessAsync(string billCode, decimal expectedAmount, string accountNumber)
    {
        try
        {
            // Lấy giao dịch trong 24h gần đây
            var transactions = await GetRecentTransactionsAsync(
                accountNumber, 
                DateTime.UtcNow.AddHours(-24)
            );
            
            // Tìm giao dịch match với bill code và số tiền
            var matchedTx = transactions.FirstOrDefault(tx => 
                tx.TransactionContent.Contains(billCode, StringComparison.OrdinalIgnoreCase) &&
                tx.AmountInDecimal == expectedAmount &&
                tx.AccountNumber == accountNumber
            );
            
            if (matchedTx != null)
            {
                _logger.LogInformation($"Payment verified: Transaction {matchedTx.Id}, Amount: {matchedTx.AmountInDecimal}");
                return true;
            }
            
            _logger.LogWarning($"No matching payment found for bill: {billCode}, amount: {expectedAmount}");
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking payment success");
            return false;
        }
    }
}

