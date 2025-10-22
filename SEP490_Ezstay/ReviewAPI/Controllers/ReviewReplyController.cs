using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using ReviewAPI.DTO.Requests.ReviewReply;
using ReviewAPI.DTO.Response.ReviewReply;
using ReviewAPI.Service;
using ReviewAPI.Service.Interface;

namespace ReviewAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewReplyController: ControllerBase
{
    private readonly IReviewReplyService _reviewReplyService;
    private readonly ITokenService _tokenService;
    public ReviewReplyController(IReviewReplyService reviewReplyService, ITokenService tokenService)
    {
        _reviewReplyService = reviewReplyService;
        _tokenService = tokenService;
    }
    [HttpPost("{reviewId}")]
    [Authorize(Roles = "Owner")]
    public async Task<IActionResult> Create(Guid reviewId, [FromForm] CreateReviewReplyRequest request)
    {
            var ownerId = _tokenService.GetUserIdFromClaims(User);
            var create = await _reviewReplyService.AddAsync(reviewId, ownerId, request);
            if (!create.IsSuccess)
                return BadRequest(new { message = create.Message });
            return CreatedAtAction("GetById", new { id = create.Data.Id }, create);
      
    }
    
    [HttpPut("{id}")]
    [Authorize(Roles = "Owner")]
    public async Task<IActionResult> Update(Guid id, [FromForm] UpdateReviewReplyRequest request)
    {
        var result = await _reviewReplyService.UpdateReplyAsync(id, request);
        if (!result.IsSuccess  )
            return BadRequest(new { message = result.Message });
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Owner")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _reviewReplyService.DeleteReplyAsync(id);
        return NoContent();
    }
    
    [HttpGet]
    [EnableQuery]
    [Authorize(Roles = "Owner,User")]
    public IQueryable<ReviewReplyResponse> GetReviewReplies()
    {
        return _reviewReplyService.GetAllQueryable();
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var reply = await _reviewReplyService.GetByIdAsync(id);
        if (reply == null) return NotFound();
        return Ok(reply);
    }
    [HttpGet("review/{id}")]
    public async Task<IActionResult> GetReplyByReviewId(Guid id)
    {
        var reply = await _reviewReplyService.GetReplyByReviewIdAsync(id);
        if (reply == null) return NotFound();
        return Ok(reply);
    }

   

}