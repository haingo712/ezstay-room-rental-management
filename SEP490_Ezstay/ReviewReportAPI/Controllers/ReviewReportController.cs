using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using ReviewReportAPI.DTO.Requests;
using ReviewReportAPI.DTO.Response;
using ReviewReportAPI.Service.Interface;

namespace ReviewReportAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewReportController: ControllerBase
{
    private readonly IReviewReportService _reviewReportService;

    public ReviewReportController(IReviewReportService reviewReportService)
    {
        _reviewReportService = reviewReportService;
    }

    [HttpGet]
    [EnableQuery]
    [Authorize(Roles = "Staff")]
    public IQueryable<ReviewReportResponse> GetAll()
    {
        return _reviewReportService.GetAll();
    }
    [HttpGet("{reviewId}")]
    public async Task<ReviewReportResponse>  GetById(Guid reviewId)
    {
        return await _reviewReportService.GetById(reviewId);
    }
    // Chủ trọ gửi báo cáo
    [HttpPost("{reviewId}")]
    [Authorize(Roles = "Owner")]
    public async Task<IActionResult> Add(Guid reviewId, [FromForm] CreateReviewReportRequest request)
    {
        var result = await _reviewReportService.Add(reviewId, request);
        if (!result.IsSuccess)
            return BadRequest(result);
        return Ok(result);
    }
    [HttpPut("{reviewId}")]
    [Authorize(Roles = "Owner")]
    public async Task<IActionResult> Update(Guid reviewId, [FromForm] UpdateReviewReportRequest request)
    {
        var result = await _reviewReportService.Update(reviewId, request);
        if (!result.IsSuccess)
            return BadRequest(result);
        return Ok(result);
    }

    // Nhân viên duyệt báo cáo
    [HttpPut("status/{id}")]
    [Authorize(Roles = "Staff")]
    public async Task<IActionResult> SetStatus(Guid id, [FromBody] UpdateReportStatusRequest request)
    {
        var result = await _reviewReportService.SetStatus(id, request);
        if (!result.IsSuccess)
            return BadRequest(result);
        return Ok(result);
    }

    
}
