using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using ReviewAPI.Service;
using ReviewAPI.Service.Interface;
using ReviewReportAPI.DTO.Requests;
using ReviewReportAPI.DTO.Response;
using ReviewReportAPI.Service.Interface;

namespace ReviewReportAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewReportController : ControllerBase
{
    private readonly IReviewReportService _reviewReportService;
    private readonly ITokenService _tokenService;
    public ReviewReportController(IReviewReportService reviewReportService, ITokenService tokenService)
    {
        _reviewReportService = reviewReportService;
        _tokenService = tokenService;
    }
    [HttpGet]
    [EnableQuery]
    [Authorize(Roles = "Staff")]
    public IQueryable<ReviewReportResponse>  GetAll()
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
     //   var ownerId = ...; // lấy từ token
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
    [HttpPut("status/{reportId}")]
    [Authorize(Roles = "Staff")]
    public async Task<IActionResult> SetStatus(Guid reportId, [FromBody] UpdateReportStatusRequest request)
    {
        var result = await _reviewReportService.SetStatus(reportId, request);
        if (!result.IsSuccess)
            return BadRequest(result);
        return Ok(result);
    }

    
}
