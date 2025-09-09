using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReviewAPI.DTO.Requests;
using ReviewAPI.DTO.Response;
using ReviewAPI.Service.Interface;

namespace ReviewAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewController : ControllerBase
{
    private readonly IReviewService _reviewService;

    public ReviewController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }
    
    // [HttpGet]
    // public async Task<IActionResult> GetAll()
    // {
    //     var result = await _reviewService.GetAll();
    //     return Ok(result);
    // }
    
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

   
    [Authorize(Roles = "User")]
    [HttpPost]
    public async Task<IActionResult> Create(Guid postId, [FromBody] CreateReviewDto request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _reviewService.AddAsync(postId, request);
        return Ok(result);
    }
    
    [Authorize]
    [Authorize(Roles = "User")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateReviewDto request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _reviewService.UpdateAsync(id, request);
        if (!result.IsSuccess) return BadRequest(result);

        return Ok(result);
    }
    
    // [Authorize]
    // [HttpDelete("{id}")]
    // public async Task<IActionResult> Delete(Guid id)
    // {
    //     await _reviewService.DeleteAsync(id);
    //     return NoContent();
    // }
}
