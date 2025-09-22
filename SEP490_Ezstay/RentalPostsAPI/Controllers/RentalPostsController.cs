
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using RentalPostsAPI.DTO.Request;
using RentalPostsAPI.DTO.Response;
using RentalPostsAPI.Service.Interface;
using System.Security.Claims;

namespace RentalPostsAPI.Controllers
{
    //[ApiController]
    [Route("api/[controller]")]
    public class RentalPostsController : ODataController
    {
        private readonly IRentalPostService _service;
        private readonly ITokenService _tokenService;

        public RentalPostsController(IRentalPostService service, ITokenService tokenService)
        {
            _service = service;
            _tokenService = tokenService;
        }

        [HttpGet]
        [EnableQuery]
        [Route("/odata/RentalPosts")]
        public IQueryable<RentalpostDTO> GetOdata()
        {
            return _service.GetAllAsQueryable();
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRentalPostDTO dto)
        {

 
            var result = await _service.CreateAsync (dto, User);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            
            return Ok(ApiResponse<IEnumerable<RentalpostDTO>>.Success(result));
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
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRentalPostDTO dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            if (result == null)
                return NotFound(ApiResponse<string>.Fail("Không tìm thấy bài viết"));
            return Ok(ApiResponse<RentalpostDTO>.Success(result, "Cập nhật thành công"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, [FromQuery] Guid deletedBy)
        {
            var success = await _service.DeleteAsync(id, deletedBy);
            if (!success)
                return NotFound(ApiResponse<string>.Fail("Không tìm thấy bài viết"));
            return Ok(ApiResponse<string>.Success("Xóa thành công"));
        }
    }
}
