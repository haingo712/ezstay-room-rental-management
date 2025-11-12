using PaymentAPI.DTOs.Requests;
using PaymentAPI.DTOs.Responses;
using Shared.DTOs;

namespace PaymentAPI.Services.Interfaces;

public interface IPaymentService
{
    Task<ApiResponse<PaymentResponse>> CreatePaymentAsync(CreatePaymentRequest request, Guid tenantId);
    Task<ApiResponse<PaymentResponse>> VerifyOnlinePaymentAsync(VerifyOnlinePaymentRequest request);
    Task<ApiResponse<string>> UploadReceiptImageAsync(Guid paymentId, Stream fileStream, string fileName);
    Task<ApiResponse<PaymentDetailResponse>> GetPaymentByIdAsync(Guid paymentId);
    Task<ApiResponse<List<PaymentInfo>>> GetPaymentsByBillIdAsync(Guid billId);
    Task<ApiResponse<List<PaymentInfo>>> GetPaymentsByUserIdAsync(Guid userId);
    Task<ApiResponse<PaymentResponse>> ApproveOfflinePaymentAsync(Guid paymentId, ApprovePaymentRequest request, Guid ownerId);
    Task<ApiResponse<PaymentResponse>> RejectOfflinePaymentAsync(Guid paymentId, RejectPaymentRequest request, Guid ownerId);
    Task<ApiResponse<List<PaymentDetailResponse>>> GetPendingApprovalsAsync(Guid ownerId);
    
    // New methods for better payment flow
    Task<ApiResponse<PaymentDetailResponse>> GetLatestPaymentByBillIdAsync(Guid billId);
    Task<ApiResponse<PaymentDetailResponse>> CheckPaymentStatusAsync(Guid paymentId);
    Task<ApiResponse<PaymentResponse>> HandleSePayWebhookAsync(string accountNumber, decimal amount, string description, string transactionId);
    
    // NEW: Get QR without creating payment (Online)
    Task<ApiResponse<PaymentQRResponse>> GetPaymentQRInfoAsync(Guid billId, Guid tenantId);
    
    // NEW: Create offline payment only
    Task<ApiResponse<PaymentResponse>> CreateOfflinePaymentAsync(Guid billId, Guid tenantId, string? notes);
    
    // NEW: Get bill payment status (for polling)
    Task<ApiResponse<BillPaymentStatusResponse>> GetBillPaymentStatusAsync(Guid billId);
    
    // NEW: Manual check payment (polling SePay API)
    Task<ApiResponse<PaymentResponse>> CheckPaymentManualAsync(Guid billId, Guid tenantId);
}
