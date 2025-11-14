namespace PaymentAPI.DTOs.Responses;

public class PaymentQRResponse
{
    public string BillId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string BankName { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public string AccountName { get; set; } = string.Empty;
    public string TransactionContent { get; set; } = string.Empty;
    public string QRCodeUrl { get; set; } = string.Empty;
}
