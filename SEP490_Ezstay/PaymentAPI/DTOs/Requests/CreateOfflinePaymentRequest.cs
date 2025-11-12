namespace PaymentAPI.DTOs.Requests;

/// <summary>
/// Request để tạo payment cho thanh toán Offline
/// </summary>
public class CreateOfflinePaymentRequest
{
    /// <summary>
    /// Ghi chú về thanh toán (optional)
    /// </summary>
    public string? Notes { get; set; }
}
