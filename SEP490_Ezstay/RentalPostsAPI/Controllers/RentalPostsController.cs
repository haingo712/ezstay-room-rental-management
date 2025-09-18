using BoardingHouseAPI.DTO.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using RentalPostsAPI.DTO.Request;

using RentalPostsAPI.Service.Interface;

namespace RentalPostsAPI.Controllers
{
    //[ApiController]
    [Route("api/[controller]")]
    public class RentalPostsController : ODataController
    {
        private readonly IRentalPostService _service;

        public RentalPostsController(IRentalPostService service)
        {
            _service = service;
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
            var result = await _service.CreateAsync(dto);
            return Ok(ApiResponse<RentalpostDTO>.Success(result, "Tạo bài viết thành công"));
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
