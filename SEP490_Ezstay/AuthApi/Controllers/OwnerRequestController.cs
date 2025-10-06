using AuthApi.DTO.Request;
using AuthApi.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerRequestController : ControllerBase
    {
        private readonly IOwnerRequestService _service;

        public OwnerRequestController(IOwnerRequestService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> SubmitOwnerRequest([FromBody] SubmitOwnerRequestDto dto)
        {
            // Lấy email từ header
            if (!Request.Headers.TryGetValue("X-Email", out var email) || string.IsNullOrEmpty(email))
                return BadRequest("Thiếu thông tin email trong header.");

            var result = await _service.SubmitRequestAsync(email, dto);
            return Ok(result);
        }

        /// <summary>
        /// Staff duyệt đơn đăng ký
        /// </summary>
        [HttpPut("{id}/approve")]
        public async Task<IActionResult> ApproveRequest(Guid id)
        {
            var result = await _service.ApproveRequestAsync(id);

            if (result.StartsWith("Không")) // trả về NotFound nếu không hợp lệ
                return NotFound(result);

            return Ok(result);
        }
    }
}
