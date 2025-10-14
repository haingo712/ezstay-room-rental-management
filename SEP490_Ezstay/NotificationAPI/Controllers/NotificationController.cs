using APIGateway.Helper.Interfaces;
using AuthApi.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotificationAPI.DTOs.Resquest;
using NotificationAPI.Service.Interfaces;

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
        [HttpGet]
        public async Task<IActionResult> GetMyNotifications()
        {
            var userId = _userHelper.GetUserId(User);
            var result = await _service.GetUserNotifications(userId);
            return Ok(result);
        }


        // GET: api/notification/all
        [HttpGet("all")]
        public async Task<IActionResult> GetAllNotifications()
        {
            var result = await _service.GetAllNotifications(); // ➕ gọi Service
            return Ok(result);
        }


        // POST: api/notification
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateNotificationRequestDto dto)
        {
            var result = await _service.CreateAsync(dto); // ❌ Không lấy từ token nữa
            return Ok(result);
        }


        // PUT: api/notification/{id}/read
        [HttpPut("{id:guid}/read")]
        public async Task<IActionResult> MarkAsRead(Guid id)
        {
            var success = await _service.MarkAsRead(id);
            return success ? Ok("Đã đánh dấu là đã đọc") : NotFound("Không tìm thấy thông báo");
        }

        // DELETE: api/notification/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _service.DeleteAsync(id);
            return success ? NoContent() : NotFound("Không tìm thấy thông báo");
        }

        [HttpPost("{id:guid}/role")]
        public async Task<IActionResult> CreateNotificationByRole(Guid id, [FromQuery] RoleEnum role)
        {
            var result = await _service.CreateRoleNoti(id, role);
            if (result == null)
                return NotFound($"Không tìm thấy thông báo hoặc không có tài khoản thuộc role {role}");

            return Ok(result);
        }
    }
}
