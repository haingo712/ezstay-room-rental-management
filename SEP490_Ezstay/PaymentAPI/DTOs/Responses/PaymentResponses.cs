namespace PaymentAPI.DTOs.Responses;

public class PaymentResponse
{
    public string PaymentId { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public PaymentInstructionDto? PaymentInstruction { get; set; }
}

public class PaymentInstructionDto
{
    public string BankName { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public string AccountName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string TransactionContent { get; set; } = string.Empty;
    public string QRCodeUrl { get; set; } = string.Empty;
}

public class PaymentDetailResponse
{
    public string Id { get; set; } = string.Empty;
    public string UtilityBillId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string TenantId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    
    // Online Payment Info
    public string? TransactionId { get; set; }
    public string? BankAccountNumber { get; set; }
    public string? TransactionContent { get; set; }
    
    // Offline Payment Info
    public string? Notes { get; set; }
    public string? ApprovedBy { get; set; }
    public string? ReceiptImageUrl { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public string? RejectReason { get; set; }
}

public class BillPaymentDetailResponse
{
    public string BillId { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public string BillStatus { get; set; } = string.Empty;
    public List<PaymentInfo> Payments { get; set; } = new();
}

public class PaymentInfo
{
    public string Id { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public string? TransactionId { get; set; }
    public string? Notes { get; set; }
}

public class SePayTransactionResponse
{
    public bool Status { get; set; }
    public SePayTransactionData? Data { get; set; }
    public string? Message { get; set; }
}

public class SePayTransactionData
{
    public int Id { get; set; }
    public string TransactionNumber { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public string TransactionDate { get; set; } = string.Empty;
    public string BankBrandName { get; set; } = string.Empty;
}

public class BillPaymentStatusResponse
{
    public bool IsPaid { get; set; }
    public string? PaymentId { get; set; }
    public decimal PaidAmount { get; set; }
    public DateTime? PaidDate { get; set; }
    public string? TransactionId { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

public class SePayTransactionListResponse
{
    public int Status { get; set; }
    public List<SePayTransactionDto> Transactions { get; set; } = new();
}

public class SePayTransactionDto
{
    public string Id { get; set; } = string.Empty;
    public string BankBrandName { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public string TransactionDate { get; set; } = string.Empty;
    public string AmountOut { get; set; } = "0.00";
    public string AmountIn { get; set; } = "0.00";
    public string Accumulated { get; set; } = "0.00";
    public string TransactionContent { get; set; } = string.Empty;
    public string ReferenceNumber { get; set; } = string.Empty;
    public string? Code { get; set; }
    public string? SubAccount { get; set; }
    public string BankAccountId { get; set; } = string.Empty;
    
    // Helper properties to get decimal values
    public decimal AmountInDecimal => decimal.TryParse(AmountIn, out var result) ? result : 0;
    public decimal AmountOutDecimal => decimal.TryParse(AmountOut, out var result) ? result : 0;
}
