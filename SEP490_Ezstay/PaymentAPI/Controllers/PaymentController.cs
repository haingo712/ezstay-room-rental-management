using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentAPI.DTOs.Requests;
using PaymentAPI.Services.Interfaces;
using System.Security.Claims;

namespace PaymentAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    private readonly ILogger<PaymentController> _logger;

    public PaymentController(
        IPaymentService paymentService,
        ILogger<PaymentController> logger)
    {
        _paymentService = paymentService;
        _logger = logger;
    }

    /// <summary>
    /// Tạo payment mới (Online hoặc Offline)
    /// ⚠️ DEPRECATED for Online - Use GetPaymentQRInfo instead
    /// Chỉ dùng cho Offline payment
    /// </summary>
    [HttpPost("create")]
    [Authorize]
    public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentRequest request)
    {
        var tenantId = GetCurrentUserId();
        var result = await _paymentService.CreatePaymentAsync(request, tenantId);
        
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }
        
        return Ok(result);
    }

    /// <summary>
    /// ⭐ NEW: Lấy thông tin QR để thanh toán KHÔNG TẠO PAYMENT
    /// Payment chỉ được tạo khi webhook về (user đã chuyển khoản)
    /// </summary>
    [HttpGet("qr/{billId}")]
    [Authorize]
    public async Task<IActionResult> GetPaymentQRInfo(Guid billId)
    {
        var tenantId = GetCurrentUserId();
        var result = await _paymentService.GetPaymentQRInfoAsync(billId, tenantId);
        
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }
        
        return Ok(result);
    }

    /// <summary>
    /// ⭐ NEW: Tạo payment cho thanh toán Offline
    /// ặtDùng khi user muốn thanh toán bằng tiền m
    /// </summary>
    [HttpPost("create-offline/{billId}")]
    [Authorize]
    public async Task<IActionResult> CreateOfflinePayment(Guid billId, [FromBody] CreateOfflinePaymentRequest? request)
    {
        var tenantId = GetCurrentUserId();
        var notes = request?.Notes;
        var result = await _paymentService.CreateOfflinePaymentAsync(billId, tenantId, notes);
        
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }
        
        return Ok(result);
    }

    /// <summary>
    /// Verify online payment (check với SePay)
    /// </summary>
    [HttpPost("verify-online")]
    [Authorize]
    public async Task<IActionResult> VerifyOnlinePayment([FromBody] VerifyOnlinePaymentRequest request)
    {
        var result = await _paymentService.VerifyOnlinePaymentAsync(request);
        
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }
        
        return Ok(result);
    }

    /// <summary>
    /// Upload receipt image cho offline payment
    /// </summary>
    [HttpPost("{paymentId}/upload-receipt")]
    [Authorize]
    public async Task<IActionResult> UploadReceipt(Guid paymentId, IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest(new { isSuccess = false, message = "File không hợp lệ" });
        }

        // Validate file type
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".pdf" };
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        
        if (!allowedExtensions.Contains(extension))
        {
            return BadRequest(new { isSuccess = false, message = "Chỉ chấp nhận file ảnh (jpg, png) hoặc pdf" });
        }

        // Validate file size (max 5MB)
        if (file.Length > 5 * 1024 * 1024)
        {
            return BadRequest(new { isSuccess = false, message = "File không được vượt quá 5MB" });
        }

        using var stream = file.OpenReadStream();
        var result = await _paymentService.UploadReceiptImageAsync(paymentId, stream, file.FileName);
        
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Lấy chi tiết 1 payment
    /// </summary>
    [HttpGet("{paymentId}")]
    [Authorize]
    public async Task<IActionResult> GetPaymentById(Guid paymentId)
    {
        var result = await _paymentService.GetPaymentByIdAsync(paymentId);
        
        if (!result.IsSuccess)
        {
            return NotFound(result);
        }
        
        return Ok(result);
    }

    /// <summary>
    /// Lấy danh sách payment của 1 bill
    /// </summary>
    [HttpGet("bill/{billId}")]
    [Authorize]
    public async Task<IActionResult> GetPaymentsByBillId(Guid billId)
    {
        var result = await _paymentService.GetPaymentsByBillIdAsync(billId);
        
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }
        
        return Ok(result);
    }

    /// <summary>
    /// Lấy danh sách payment của user (tenant)
    /// </summary>
    [HttpGet("my-payments")]
    [Authorize]
    public async Task<IActionResult> GetMyPayments()
    {
        var userId = GetCurrentUserId();
        var result = await _paymentService.GetPaymentsByUserIdAsync(userId);
        
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }
        
        return Ok(result);
    }

    /// <summary>
    /// Chủ trọ duyệt offline payment
    /// </summary>
    [HttpPost("{paymentId}/approve")]
    [Authorize(Roles = "Owner,Admin")]
    public async Task<IActionResult> ApproveOfflinePayment(Guid paymentId, [FromBody] ApprovePaymentRequest request)
    {
        var ownerId = GetCurrentUserId();
        var result = await _paymentService.ApproveOfflinePaymentAsync(paymentId, request, ownerId);
        
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }
        
        return Ok(result);
    }

    /// <summary>
    /// Chủ trọ từ chối offline payment
    /// </summary>
    [HttpPost("{paymentId}/reject")]
    [Authorize(Roles = "Owner,Admin")]
    public async Task<IActionResult> RejectOfflinePayment(
        Guid paymentId, 
        [FromBody] RejectPaymentRequest request)
    {
        var ownerId = GetCurrentUserId();
        var result = await _paymentService.RejectOfflinePaymentAsync(paymentId, request, ownerId);
        
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }
        
        return Ok(result);
    }

    /// <summary>
    /// Lấy danh sách offline payment chờ duyệt (cho owner)
    /// </summary>
    [HttpGet("pending-approvals")]
    [Authorize(Roles = "Owner,Admin")]
    public async Task<IActionResult> GetPendingApprovals()
    {
        var ownerId = GetCurrentUserId();
        var result = await _paymentService.GetPendingApprovalsAsync(ownerId);
        
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }
        
        return Ok(result);
    }

    /// <summary>
    /// Lấy payment mới nhất của 1 bill (dùng cho trang BillDetail)
    /// </summary>
    [HttpGet("bill/{billId}/latest")]
    [Authorize]
    public async Task<IActionResult> GetLatestPaymentByBillId(Guid billId)
    {
        var result = await _paymentService.GetLatestPaymentByBillIdAsync(billId);
        
        if (!result.IsSuccess)
        {
            return NotFound(result);
        }
        
        return Ok(result);
    }

    /// <summary>
    /// Check trạng thái payment (dùng cho polling sau khi scan QR)
    /// </summary>
    [HttpGet("{paymentId}/status")]
    [Authorize]
    public async Task<IActionResult> CheckPaymentStatus(Guid paymentId)
    {
        var result = await _paymentService.CheckPaymentStatusAsync(paymentId);
        
        if (!result.IsSuccess)
        {
            return NotFound(result);
        }
        
        return Ok(result);
    }

    /// <summary>
    /// Webhook endpoint để nhận thông báo từ SePay khi có giao dịch mới
    /// URL: /api/Payment/webhook/sepay
    /// </summary>
    [HttpPost("webhook/sepay")]
    [AllowAnonymous] // Webhook không cần auth
    public async Task<IActionResult> SePayWebhook([FromBody] SePayWebhookRequest request)
    {
        try
        {
            // TODO: Uncomment khi SePay config webhook secret
            // // 1. Validate signature
            // var signature = Request.Headers["X-SePay-Signature"].ToString();
            // var payload = await new StreamReader(Request.Body).ReadToEndAsync();
            // 
            // var validator = new WebhookSignatureValidator(_logger, Configuration);
            // if (!validator.ValidateSignature(payload, signature))
            // {
            //     _logger.LogWarning("Invalid webhook signature");
            //     return Unauthorized(new { error = "Invalid signature" });
            // }
            //
            // // 2. Validate timestamp (prevent replay attack)
            // var timestamp = Request.Headers["X-SePay-Timestamp"].ToString();
            // if (!validator.ValidateTimestamp(timestamp))
            // {
            //     _logger.LogWarning("Invalid webhook timestamp");
            //     return BadRequest(new { error = "Invalid timestamp" });
            // }
            
            _logger.LogInformation($"🔔 Received SePay webhook: {System.Text.Json.JsonSerializer.Serialize(request)}");
            _logger.LogInformation($"📝 Content: {request.Content}");
            _logger.LogInformation($"💰 Amount: {request.TransferAmount}");
            _logger.LogInformation($"🏦 Account: {request.AccountNumber}");
            _logger.LogInformation($"🆔 Transaction ID: {request.Id}");
            
            // 3. Process webhook - Use Content field (not Description)
            var result = await _paymentService.HandleSePayWebhookAsync(
                request.AccountNumber,
                request.TransferAmount,
                request.Content,  // Use Content instead of Description
                request.Id  // Use Id instead of TransactionId
            );
            
            if (!result.IsSuccess)
            {
                _logger.LogError($"Webhook processing failed: {result.Message}");
                return BadRequest(result);
            }
            
            _logger.LogInformation($"Webhook processed successfully for transaction {request.TransactionId}");
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing webhook");
            return StatusCode(500, new { error = "Internal server error" });
        }
    }

    /// <summary>
    /// Check xem bill đã được thanh toán chưa (dùng cho polling từ frontend)
    /// Frontend có thể gọi API này mỗi 3-5s sau khi show QR
    /// </summary>
    [HttpGet("bill/{billId}/payment-status")]
    [Authorize]
    public async Task<IActionResult> GetBillPaymentStatus(Guid billId)
    {
        var result = await _paymentService.GetBillPaymentStatusAsync(billId);
        
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }
        
        return Ok(result);
    }

    /// <summary>
    /// Check bill payment status - Polling endpoint để frontend tự động check
    /// Chỉ check database, không gọi SePay API
    /// </summary>
    [HttpGet("check-payment/{billId}")]
    [Authorize]
    public async Task<IActionResult> CheckBillPaymentStatus(Guid billId)
    {
        try
        {
            // Get bill payment status from database only (not calling SePay)
            var result = await _paymentService.GetBillPaymentStatusAsync(billId);
            
            if (!result.IsSuccess)
            {
                return Ok(new { isPaid = false, message = result.Message });
            }
            
            return Ok(new 
            { 
                isPaid = result.Data?.Status == "Paid",
                status = result.Data?.Status,
                message = result.Message 
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking payment status for bill {BillId}", billId);
            return Ok(new { isPaid = false, message = "Chưa thanh toán" });
        }
    }

    /// <summary>
    /// Manual check payment - User nhấn "Đã chuyển khoản, kiểm tra ngay"
    /// API này sẽ chủ động gọi SePay để tìm giao dịch
    /// </summary>
    [HttpPost("check-payment-manual/{billId}")]
    [AllowAnonymous] // Allow without auth for testing
    public async Task<IActionResult> CheckPaymentManual(Guid billId)
    {
        _logger.LogInformation($"🔍 Manual payment check requested for bill: {billId}");
        
        // Try to get tenant ID from auth, but allow anonymous for testing
        Guid tenantId = Guid.Empty;
        try
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                tenantId = GetCurrentUserId();
                _logger.LogInformation($"👤 Authenticated Tenant ID: {tenantId}");
            }
            else
            {
                _logger.LogInformation($"⚠️ Anonymous request - will use empty tenant ID");
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"⚠️ Cannot get user ID: {ex.Message}");
        }
        
        var result = await _paymentService.CheckPaymentManualAsync(billId, tenantId);
        
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }
        
        return Ok(result);
    }

    // Helper method
    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                         ?? User.FindFirst("userId")?.Value
                         ?? User.FindFirst("sub")?.Value;
        
        if (string.IsNullOrEmpty(userIdClaim))
        {
            throw new UnauthorizedAccessException("User not authenticated");
        }

        return Guid.Parse(userIdClaim);
    }
}

// DTO for SePay Webhook - Match với format từ SePay
public class SePayWebhookRequest
{
    // SePay fields
    public string Gateway { get; set; } = string.Empty;
    public string TransactionDate { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public string? SubAccount { get; set; }
    public string? Code { get; set; }
    public string Content { get; set; } = string.Empty;
    public string TransferType { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal TransferAmount { get; set; }
    public string? ReferenceCode { get; set; }
    public decimal Accumulated { get; set; }
    public string Id { get; set; } = string.Empty; // Transaction ID
    
    // Legacy compatibility - map từ fields mới
    [System.Text.Json.Serialization.JsonIgnore]
    public decimal Amount => TransferAmount;
    
    [System.Text.Json.Serialization.JsonIgnore]
    public string TransactionId => Id;
    
    [System.Text.Json.Serialization.JsonIgnore]
    public string BankBrandName => Gateway;
}
