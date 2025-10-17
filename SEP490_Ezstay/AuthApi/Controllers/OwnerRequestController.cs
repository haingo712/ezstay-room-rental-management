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
        [HttpPost("request-owner")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> RequestBecomeOwner([FromBody] SubmitOwnerRequestClientDto clientDto)
        {
            var accountIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(accountIdClaim, out var accountId))
                return BadRequest(new { message = "Không tìm thấy thông tin tài khoản trong token." });

            var dto = new SubmitOwnerRequestDto
            {
                Reason = clientDto.Reason
            };

            var resultDto = await _service.SubmitRequestAsync(dto, accountId);

            if (resultDto == null)
                return BadRequest(new { message = "Gửi đơn thất bại" });

            return Ok(resultDto);
        }



        // Duyệt đơn (Staff/Admin)
        [HttpPut("approve/{requestId}")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> ApproveRequest(Guid requestId)
        {
            var staffIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(staffIdClaim))
                return Unauthorized(new { message = "Không xác định được staff." });

            var staffId = Guid.Parse(staffIdClaim);

            var result = await _service.ApproveRequestAsync(requestId, staffId);
            if (result == null)
                return BadRequest(new { message = "Duyệt đơn thất bại hoặc đơn không hợp lệ." });

            return Ok(result);
        }

        [HttpPut("reject/{requestId}")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> RejectOwnerRequest(Guid requestId, [FromBody] RejectOwnerRequestDto dto)
        {
            var staffIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(staffIdClaim))
                return Unauthorized(new { message = "Không xác định được staff." });

            var staffId = Guid.Parse(staffIdClaim);

            var result = await _service.RejectRequestAsync(requestId, staffId, dto.RejectionReason);
            if (result == null)
                return BadRequest(new { message = "Từ chối đơn thất bại hoặc đơn không hợp lệ." });

            return Ok(result);
        }


        [HttpGet]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> GetAllUserRequests()
        {
            var requests = await _service.GetAllRequestsAsync();
            return Ok(requests);
        }







    }
}
