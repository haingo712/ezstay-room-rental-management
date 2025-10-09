using AuthApi.DTO.Request;
using AuthApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        // ✅ SubmitOwnerRequest không phân quyền, chỉ cần JWT hợp lệ
        [HttpPost]
        public async Task<IActionResult> SubmitOwnerRequest([FromBody] SubmitOwnerRequestDto dto)
        {
            // Lấy accountId từ dto hoặc service, không dùng Claim nếu không có
            var resultDto = await _service.SubmitRequestAsync(dto);

            if (resultDto == null)
                return BadRequest(new { message = "Gửi đơn thất bại" });

            return Ok(resultDto);
        }


        // Duyệt đơn (Staff/Admin)
        [HttpPut("{id}/approve")]
        public async Task<IActionResult> ApproveRequest(Guid id)
        {
            var result = await _service.ApproveRequestAsync(id);

            if (result.StartsWith("Không"))
                return NotFound(result);

            return Ok(result);
        }
    }
}
