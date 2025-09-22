
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentalPostsAPI.DTO.Request;
using RentalPostsAPI.DTO.Response;
using RentalPostsAPI.Service.Interface;
using System.Security.Claims;

namespace RentalPostsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
 
    public class RentalPostsController : ControllerBase
    {
        private readonly IRentalPostService _service;
        private readonly ITokenService _tokenService;

        public RentalPostsController(IRentalPostService service, ITokenService tokenService)
        {
            _service = service;
            _tokenService = tokenService;
        }

        [HttpPost]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> Create([FromBody] CreateRentalPostDTO dto)
        {

 
            var result = await _service.CreateAsync (dto, User);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllForUser()
        {
            var result = await _service.GetAllForUserAsync();
            return Ok(result);
        }
        [HttpGet("owner")]
        [Authorize(Roles = "Owner")] 
        public async Task<IActionResult> GetAllForOwner()
        {
           
            var result = await _service.GetAllForOwnerAsync(User);
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
                return NotFound(ApiResponse<string>.Fail("Không tìm thấy bài viết"));
            return Ok(ApiResponse<RentalpostDTO>.Success(result));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRentalPostDTO dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            if (result == null)
                return NotFound(ApiResponse<string>.Fail("Không tìm thấy bài viết"));
            return Ok(ApiResponse<RentalpostDTO>.Success(result, "Cập nhật thành công"));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> Delete(Guid id, [FromQuery] Guid deletedBy)
        {
            var success = await _service.DeleteAsync(id, deletedBy);
            if (!success)
                return NotFound(ApiResponse<string>.Fail("Không tìm thấy bài viết"));
            return Ok(ApiResponse<string>.Success("Xóa thành công"));
        }
    }
}
