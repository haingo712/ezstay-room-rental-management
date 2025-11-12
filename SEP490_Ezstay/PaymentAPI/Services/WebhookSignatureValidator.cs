using System.Security.Cryptography;
using System.Text;

namespace PaymentAPI.Services;

/// <summary>
/// Service để validate webhook signature từ SePay
/// Đảm bảo webhook request thực sự từ SePay, không phải từ nguồn giả mạo
/// </summary>
public class WebhookSignatureValidator
{
    private readonly ILogger<WebhookSignatureValidator> _logger;
    private readonly string _webhookSecret;

    public WebhookSignatureValidator(
        ILogger<WebhookSignatureValidator> logger,
        IConfiguration configuration)
    {
        _logger = logger;
        _webhookSecret = configuration["SePay:WebhookSecret"] ?? "";
    }

    /// <summary>
    /// Validate webhook signature
    /// </summary>
    /// <param name="payload">Request body JSON</param>
    /// <param name="signatureHeader">Signature từ header (X-SePay-Signature)</param>
    /// <returns>True nếu signature hợp lệ</returns>
    public bool ValidateSignature(string payload, string signatureHeader)
    {
        if (string.IsNullOrEmpty(_webhookSecret))
        {
            _logger.LogWarning("Webhook secret not configured - skipping validation");
            return true; // Skip validation nếu chưa config
        }

        if (string.IsNullOrEmpty(signatureHeader))
        {
            _logger.LogWarning("Missing signature header");
            return false;
        }

        try
        {
            // Tính toán signature từ payload
            var computedSignature = ComputeHMACSHA256(payload, _webhookSecret);
            
            // So sánh signature
            var isValid = signatureHeader.Equals(computedSignature, StringComparison.OrdinalIgnoreCase);
            
            if (!isValid)
            {
                _logger.LogWarning(
                    "Invalid webhook signature. Expected: {Expected}, Got: {Got}",
                    computedSignature,
                    signatureHeader
                );
            }
            
            return isValid;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating webhook signature");
            return false;
        }
    }

    /// <summary>
    /// Tính toán HMAC-SHA256 signature
    /// </summary>
    private string ComputeHMACSHA256(string message, string secret)
    {
        var encoding = new UTF8Encoding();
        var keyBytes = encoding.GetBytes(secret);
        var messageBytes = encoding.GetBytes(message);
        
        using var hmac = new HMACSHA256(keyBytes);
        var hashBytes = hmac.ComputeHash(messageBytes);
        
        // Convert to hex string
        return BitConverter.ToString(hashBytes)
            .Replace("-", "")
            .ToLowerInvariant();
    }

    /// <summary>
    /// Validate timestamp để tránh replay attack
    /// Chỉ chấp nhận webhook trong vòng 5 phút
    /// </summary>
    public bool ValidateTimestamp(string timestampHeader)
    {
        if (string.IsNullOrEmpty(timestampHeader))
        {
            _logger.LogWarning("Missing timestamp header");
            return false;
        }

        try
        {
            if (!long.TryParse(timestampHeader, out var timestamp))
            {
                _logger.LogWarning("Invalid timestamp format: {Timestamp}", timestampHeader);
                return false;
            }

            var requestTime = DateTimeOffset.FromUnixTimeSeconds(timestamp);
            var currentTime = DateTimeOffset.UtcNow;
            var timeDifference = Math.Abs((currentTime - requestTime).TotalMinutes);

            // Chỉ chấp nhận request trong vòng 5 phút
            if (timeDifference > 5)
            {
                _logger.LogWarning(
                    "Webhook timestamp too old: {RequestTime} (now: {CurrentTime})",
                    requestTime,
                    currentTime
                );
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating timestamp");
            return false;
        }
    }
}
