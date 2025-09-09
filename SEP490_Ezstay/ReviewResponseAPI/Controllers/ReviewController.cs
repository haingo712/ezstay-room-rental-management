using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReviewResponseAPI.DTO.Requests;
using ReviewResponseAPI.DTO.Response;
using ReviewResponseAPI.Service.Interface;

namespace ReviewResponseAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewResponseController : ControllerBase
{
    private readonly IReviewResponseService _service;

    public ReviewResponseController(IReviewResponseService service)
    {
        _service = service;
    }
    
    [HttpGet("mine")]
    public async Task<IActionResult> GetAllByOwnerId()
    {
        var result = await _service.GetAllByOwnerId();
        return Ok(result);
    }

    
    [HttpGet]
    [Authorize(Roles = "Staff")]
    public async Task<IActionResult> GetAllByStaffId()
    {
        var result = await _service.GetAll();
        return Ok(result);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var dto = await _service.GetByIdAsync(id);
        return Ok(dto);
    }

   
    [HttpPost]
    [Authorize(Roles = "Owner")]
    public async Task<IActionResult> Create(Guid reviewId, [FromBody] CreateReviewResponseDto request)
    {
        var result = await _service.AddAsync(reviewId, request);
        return Ok(result);
    }
    
    [HttpPut("{id}")]
    [Authorize(Roles = "Owner")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateReviewResponseDto request)
    {
        var result = await _service.UpdateAsync(id, request);
        return Ok(result);
    }

   
    [HttpDelete("{id}")]
    [Authorize(Roles = "Owner")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
