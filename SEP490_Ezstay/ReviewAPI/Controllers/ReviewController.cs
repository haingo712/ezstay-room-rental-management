using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using ReviewAPI.DTO.Requests;
using ReviewAPI.DTO.Response;
using ReviewAPI.Service;
using ReviewAPI.Service.Interface;

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
    // // [Authorize(Roles = "Staff")]
    // public async Task<IActionResult> GetAll()
    // {
    //     var result = await _reviewService.GetAll();
    //     return Ok(result);
    // }
    [HttpGet]
    [EnableQuery]
    [Route("/odata/Review")]
    [Authorize(Roles = "Owner")]
    public IQueryable<ReviewResponseDto> GetAllAsQueryable()
    {
        return _reviewService.GetAllAsQueryable();
    }
    [HttpGet]
    [EnableQuery]
    public IQueryable<ReviewResponseDto> GetAll()
    {
        return _reviewService.GetAllAsQueryable();
    }
    [HttpGet("post/{postId}")]
    public async Task<IActionResult> GetByPostId(Guid postId)
    {
        var result = await _reviewService.GetAllByPostId(postId);
        return Ok(result);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _reviewService.GetByIdAsync(id);
        return Ok(result);
    }
    
    // [Authorize(Roles = "User")]
    // [HttpPost("Post/{postId}")]
    // public async Task<IActionResult> Create(Guid postId, [FromBody] CreateReviewDto request)
    // {
    //     var userId = _tokenService.GetUserIdFromClaims(User);
    //     if (!ModelState.IsValid) return BadRequest(ModelState);
    //
    //     var result = await _reviewService.AddAsync(userId, postId, request);
    //     return Ok(result);
    // }
    [Authorize(Roles = "User")]
    [HttpPost("{contractId}")]
    public async Task<IActionResult> Create(Guid contractId, [FromForm] CreateReviewDto request)
    {
        var userId = _tokenService.GetUserIdFromClaims(User);
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _reviewService.AddAsync(userId, contractId,request);
        return Ok(result);
    }
    
    [Authorize(Roles = "User")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromForm] UpdateReviewDto request)
    {
        var userId = _tokenService.GetUserIdFromClaims(User);
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _reviewService.UpdateAsync(id, userId, request);
        if (!result.IsSuccess) return BadRequest(result);

        return Ok(result);
    }
    
    [Authorize(Roles = "Staff")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _reviewService.DeleteAsync(id);
        return NoContent();
    }
}
