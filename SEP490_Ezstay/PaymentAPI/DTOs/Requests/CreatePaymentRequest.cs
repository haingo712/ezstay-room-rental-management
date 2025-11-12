using Shared.Enums;

namespace PaymentAPI.DTOs.Requests;

public class CreatePaymentRequest
{
    public Guid UtilityBillId { get; set; }
    public PaymentMethod PaymentMethod { get; set; } 
 //   public string? Notes { get; set; }
}

public class VerifyOnlinePaymentRequest
{
    public Guid PaymentId { get; set; }
    public string TransactionId { get; set; } 
}
