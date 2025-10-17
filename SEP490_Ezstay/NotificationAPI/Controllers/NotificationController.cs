using APIGateway.Helper.Interfaces;
using AuthApi.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using NotificationAPI.DTOs.Resquest;
using NotificationAPI.Service.Interfaces;
using NotificationAPI.Sinair;
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

        private readonly IHubContext<NotificationHub> _hubContext; // ✅ thêm HubContext

        public NotificationController(INotificationService service, IUserClaimHelper userHelper, IHubContext<NotificationHub> hubContext)
        {
            _service = service;
            _userHelper = userHelper;
            _hubContext = hubContext;
        }


        // GET: api/notification
        private Guid GetUserIdFromToken()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                throw new UnauthorizedAccessException("Token không hợp lệ hoặc thiếu userId.");
            return Guid.Parse(userIdClaim);
        }

        //[HttpGet]
        //public async Task<IActionResult> GetAll()
        //{
        //    var userId = GetUserIdFromToken();
        //    var result = await _service.GetAllByUserAsync(userId);
          

        //    return Ok(result);
        //}

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
            await _hubContext.Clients.User(userId.ToString())
             .SendAsync("ReceiveNotification", result);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] NotifyRequest request)
        {
            var result = await _service.UpdateAsync(id, request);
            if (result == null) return NotFound();
            await _hubContext.Clients.All.SendAsync("UpdateNotification", result);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            await _hubContext.Clients.All.SendAsync("DeleteNotification", id);
            return NoContent();
        }


        [HttpPost("by-role")]
        public async Task<IActionResult> CreateByRole([FromBody] NotifyByRoleRequest request)
        {
            var result = await _service.CreateByRoleAsync(request);
            await _hubContext.Clients.Group(request.TargetRole.ToString())
                .SendAsync("ReceiveRoleNotification", result);
            return Ok(result);
        }

        [HttpPut("mark-read/{id}")]
        public async Task<IActionResult> MarkAsRead(Guid id)
        {
            var success = await _service.MarkAsReadAsync(id);
            await _hubContext.Clients.All.SendAsync("MarkAsRead", id);
            return success ? Ok("Đã đánh dấu đã đọc") : NotFound();
        }

        [HttpPut("UpdateByrole{id}")]
        public async Task<IActionResult> UpdateByRole(Guid id, [FromBody] NotifyRequest request)
        {
            var result = await _service.UpdateAsync(id, request);
            if (result == null) return NotFound();
            return Ok(result);
        }


        [HttpGet("all-by-role")]
        public async Task<IActionResult> GetAllByRoleOrUser()
        {
            var userId = GetUserIdFromToken();
            var roleClaim = User.FindFirst(ClaimTypes.Role)?.Value;
            if (string.IsNullOrEmpty(roleClaim))
                return Unauthorized("Không tìm thấy thông tin vai trò trong token.");

            if (!Enum.TryParse<RoleEnum>(roleClaim, out var role))
                return BadRequest("Role không hợp lệ.");


            var result = await _service.GetAllByRoleOrUserAsync(userId, role);
            return Ok(result);
        }




    }

}
