using APIGateway.Helper.Interfaces;
using AuthApi.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotificationAPI.DTOs.Resquest;
using NotificationAPI.Service.Interfaces;
using System.Security.Claims;

namespace NotificationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _service;
        private readonly IUserClaimHelper _userHelper;

        public NotificationController(INotificationService service, IUserClaimHelper userHelper)
        {
            _service = service;
            _userHelper = userHelper;
        }

        // GET: api/notification
        private Guid GetUserIdFromToken()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                throw new UnauthorizedAccessException("Token không hợp lệ hoặc thiếu userId.");
            return Guid.Parse(userIdClaim);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = GetUserIdFromToken();
            var result = await _service.GetAllByUserAsync(userId);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] NotifyRequest request)
        {
            var userId = GetUserIdFromToken();
            var result = await _service.CreateAsync(userId, request);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] NotifyRequest request)
        {
            var result = await _service.UpdateAsync(id, request);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }


        [HttpPost("by-role")]
        public async Task<IActionResult> CreateByRole([FromBody] NotifyByRoleRequest request)
        {
            var result = await _service.CreateByRoleAsync(request);
            return Ok(result);
        }

        [HttpPut("mark-read/{id}")]
        public async Task<IActionResult> MarkAsRead(Guid id)
        {
            var success = await _service.MarkAsReadAsync(id);
            return success ? Ok("Đã đánh dấu đã đọc") : NotFound();
        }

        [HttpPut("by-role")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> UpdateByRole([FromBody] NotifyByRoleRequest request)
        {
            var result = await _service.UpdateByRoleAsync(request);
            return Ok(result);
        }



    }

}
