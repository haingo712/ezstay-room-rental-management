using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using ReviewAPI.DTO.Requests;
using ReviewAPI.DTO.Response;
using ReviewAPI.Repository.Interface;
using ReviewAPI.Service;
using ReviewAPI.Service.Interface;
using Shared.DTOs;
using Shared.DTOs.Reviews.Responses;

namespace ReviewAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewController : ControllerBase
{
    private readonly IReviewService _reviewService;
    private readonly ITokenService _tokenService;

    public ReviewController(IReviewService reviewService, ITokenService tokenService)
    {
        _reviewService = reviewService;
        _tokenService = tokenService;
    }
    // [HttpGet]
    // [EnableQuery]
    // public IQueryable<ReviewResponse> GetAll()
    // {
    //     return _reviewService.GetAll();
    //     
    // }
    
    [HttpGet]
    [EnableQuery]
    public IQueryable<ReviewResponse> GetAllByOwnerId()
    {
        var ownerId = _tokenService.GetUserIdFromClaims(User);
        return _reviewService.GetAllByOwnerId(ownerId);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _reviewService.GetById(id);
        return Ok(result);
    }
    
    [Authorize(Roles = "User")]
    [HttpPost("{contractId}")]
    public async Task<IActionResult> Create(Guid contractId, [FromForm] CreateReviewRequest request)
    {
        var userId = _tokenService.GetUserIdFromClaims(User);
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
       
        var result = await _reviewService.Add(userId, contractId, request);
        if (!result.IsSuccess)
        {
            return BadRequest(new { message = result.Message });
        }
        return Ok(result);
    }

    [Authorize(Roles = "User")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromForm] UpdateReviewRequest request)
    {
        var userId = _tokenService.GetUserIdFromClaims(User);
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _reviewService.Update(id, request);
        if (!result.IsSuccess) return BadRequest(result);

        return Ok(result);
    }
    
    [Authorize(Roles = "Staff")]
    [HttpPut("{id}/hide/{hide}")]
    public async Task<IActionResult> HideReview(Guid id, bool hide)
    {
        var result = await _reviewService.HideReview(id, hide);
        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }
   
    [HttpGet("{contractId}/check-exists")]
    public async Task<IActionResult> ReviewExistsByContractId(Guid contractId)
    {
        var result= await _reviewService.ReviewExistsByContractId(contractId);
        return Ok(result);
    }
    [HttpGet("by-room")]
    public async Task<IActionResult> GetReviewsByRoom([FromQuery] List<Guid> roomIds)
    {
        if (roomIds == null || !roomIds.Any())
            return Ok(new List<ReviewResponse>());

        var reviews = await _reviewService.GetByRoomIdsAsync(roomIds);
        return Ok(reviews);
    }
}
