using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReviewReportAPI.DTO.Requests;
using ReviewReportAPI.Service.Interface;

namespace ReviewReportAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewReportController : ControllerBase
{
    private readonly IReviewReportService _reviewReportService;

    public ReviewReportController(IReviewReportService reviewReportService)
    {
        _reviewReportService = reviewReportService;
    }

    // Chủ trọ gửi báo cáo
    [HttpPost("{reviewId}")]
    [Authorize(Roles = "Owner")]
    public async Task<IActionResult> Create(Guid reviewId, [FromBody] CreateReviewReportRequest request)
    {
        // var ownerId = ...; // lấy từ token
        var result = await _reviewReportService.AddAsync(reviewId, request);
        if (!result.IsSuccess)
            return BadRequest(result);
        return Ok(result);
    }

    // Nhân viên duyệt báo cáo
    [HttpPut("{id}/approve")]
    [Authorize(Roles = "Staff")]
    public async Task<IActionResult> Approve(Guid id)
    {
        var result = await _reviewReportService.ApproveAsync(id);
        return Ok(result);
    }

    [HttpPut("{id}/reject")]
    [Authorize(Roles = "Staff")]
    public async Task<IActionResult> Reject(Guid id, [FromBody] string reason)
    {
        var result = await _reviewReportService.RejectAsync(id, reason);
        return Ok(result);
    }

    // Staff xem danh sách report
    [HttpGet]
    [Authorize(Roles = "Staff")]
    public async Task<IActionResult> GetAll()
    {
        var reports = await _reviewReportService.GetAllAsync();
        return Ok(reports);
    }
}
