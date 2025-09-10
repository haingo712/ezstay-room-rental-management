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
    private readonly ITokenService _tokenService;
    public ReviewResponseController(IReviewResponseService service, ITokenService tokenService)
    {
        _service = service;
        _tokenService = tokenService;
    }
    
    [HttpGet("ByOwnerId")]
    [Authorize(Roles = "Owner")]
    public async Task<IActionResult> GetAllByOwnerId()
    {
        var accountId = _tokenService.GetUserIdFromClaims(User);
        var result = await _service.GetAllByOwnerId(accountId);
        return Ok(result);
    }

    
    [HttpGet]
    public async Task<IActionResult> GetAll()
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
        var accountId = _tokenService.GetUserIdFromClaims(User);
        var result = await _service.AddAsync(reviewId, accountId , request);
        return Ok(result);
    }
    
    [HttpPut("{id}")]
    [Authorize(Roles = "Owner")]
    public async Task<IActionResult> Update(Guid id,[FromBody] UpdateReviewResponseDto request)
    {
        var accountId = _tokenService.GetUserIdFromClaims(User);
        var result = await _service.UpdateAsync(id, accountId,  request);
        return Ok(result);
    }

   
    [HttpDelete("{id}")]
    [Authorize(Roles = "Owner")]
    public async Task<IActionResult> Delete(Guid id)
    {   var accountId = _tokenService.GetUserIdFromClaims(User);
        await _service.DeleteAsync(id, accountId);
        return NoContent();
    }
}
