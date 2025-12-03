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
 
    public PaymentController(
        IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    /// <summary>
    /// Webhook endpoint cho SePay
    /// </summary>
    [HttpPost("hook/sepay")]
    [AllowAnonymous]
    public async Task<IActionResult> SePayWebhook([FromBody] CreatePayment request)
    {
        var result = await _paymentService.HandleSePayWebhookAsync(request);
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }
    
    /// <summary>
    /// Lấy lịch sử thanh toán của người thuê hiện tại (từ JWT)
    /// </summary>
    // [HttpGet("my-history")]
    // [Authorize]
    // public async Task<IActionResult> GetMyPaymentHistory()
    // {
    //     var userId = GetCurrentUserId();
    //     var result = await _paymentService.GetPaymentsByTenantIdAsync(userId);
    //     
    //     if (!result.IsSuccess)
    //     {
    //         return BadRequest(result);
    //     }
    //     
    //     return Ok(result);
    // }
    
    /// <summary>
    /// Lấy lịch sử thanh toán theo TenantId (cho Admin/Staff)
    /// </summary>
    // [HttpGet("history/tenant/{tenantId}")]
    // [Authorize]
    // public async Task<IActionResult> GetPaymentsByTenantId(Guid tenantId)
    // {
    //     var result = await _paymentService.GetPaymentsByTenantIdAsync(tenantId);
    //     
    //     if (!result.IsSuccess)
    //     {
    //         return BadRequest(result);
    //     }
    //     
    //     return Ok(result);
    // }
    //
    // /// <summary>
    // /// Lấy lịch sử thanh toán của chủ trọ hiện tại (từ JWT)
    // /// </summary>
    // [HttpGet("owner/received")]
    // [Authorize]
    // public async Task<IActionResult> GetOwnerReceivedPayments()
    // {
    //     var userId = GetCurrentUserId();
    //     var result = await _paymentService.GetPaymentsByOwnerIdAsync(userId);
    //     
    //     if (!result.IsSuccess)
    //     {
    //         return BadRequest(result);
    //     }
    //     
    //     return Ok(result);
    // }
    //
    // /// <summary>
    // /// Lấy lịch sử thanh toán theo OwnerId (cho Admin/Staff)
    // /// </summary>
    // [HttpGet("history/owner/{ownerId}")]
    // [Authorize]
    // public async Task<IActionResult> GetPaymentsByOwnerId(Guid ownerId)
    // {
    //     var result = await _paymentService.GetPaymentsByOwnerIdAsync(ownerId);
    //     
    //     if (!result.IsSuccess)
    //     {
    //         return BadRequest(result);
    //     }
    //     
    //     return Ok(result);
    // }
    //
    // /// <summary>
    // /// Lấy lịch sử thanh toán theo BillId
    // /// </summary>
    // [HttpGet("history/bill/{billId}")]
    // [Authorize]
    // public async Task<IActionResult> GetPaymentsByBillId(Guid billId)
    // {
    //     var result = await _paymentService.GetPaymentsByBillIdAsync(billId);
    //     
    //     if (!result.IsSuccess)
    //     {
    //         return BadRequest(result);
    //     }
    //     
    //     return Ok(result);
    // }
    
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
