using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using ReviewAPI.DTO.Requests.ReviewReply;
using ReviewAPI.DTO.Response.ReviewReply;
using ReviewAPI.Service.Interface;

namespace ReviewAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewReplyController: ControllerBase
{
    private readonly IReviewReplyService _reviewReplyService;
    public ReviewReplyController(IReviewReplyService reviewReplyService)
    {
        _reviewReplyService = reviewReplyService;
    }

    [HttpPost("{id}")]
    [Authorize(Roles = "Owner")]
    public async Task<IActionResult> Create(Guid id, [FromBody] CreateReviewReplyRequest request)
    {
            var create = await _reviewReplyService.AddAsync(id, request);
            if (!create.IsSuccess)
                return BadRequest(new { message = create.Message });
            return CreatedAtAction("GetById", new { id = create.Data.Id }, create);
      
    }
    
    [HttpPut("{id}")]
    [Authorize(Roles = "Owner")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateReviewReplyRequest request)
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