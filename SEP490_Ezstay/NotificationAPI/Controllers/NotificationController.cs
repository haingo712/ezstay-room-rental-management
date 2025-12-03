using APIGateway.Helper.Interfaces;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using NotificationAPI.DTOs.Resquest;
using NotificationAPI.Service.Interfaces;
using NotificationAPI.Sinair;
using Shared.Enums;
using System.Security.Claims;

namespace NotificationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
                throw new UnauthorizedAccessException("Token is invalid or userId is missing.");
            return Guid.Parse(userIdClaim);
        }

        [HttpGet("types")]
        [Authorize(Roles = "Admin,Staff,Owner")]
        public IActionResult GetNotificationTypes()
        {
            var types = _service.GetAllNotificationTypes();
            return Ok(types);
        }

        [HttpGet("roles")]
        [Authorize(Roles = "Admin,Staff,Owner")]
        public IActionResult GetAllRoles()
        {
            var roles = _service.GetAllRoles();
            return Ok(roles);
        }


        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Staff,Owner")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Staff,Owner")]
        public async Task<IActionResult> Create([FromBody] NotifyRequest request)
        {
            var userId = GetUserIdFromToken();
            var result = await _service.CreateAsync(userId, request);
            await _hubContext.Clients.User(userId.ToString())
             .SendAsync("ReceiveNotification", result);
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Staff,Owner")]
        public async Task<IActionResult> Update(Guid id, [FromBody] NotifyRequest request)
        {
            var result = await _service.UpdateAsync(id, request);
            if (result == null) return NotFound();
            await _hubContext.Clients.All.SendAsync("UpdateNotification", result);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Staff,Owner")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            await _hubContext.Clients.All.SendAsync("DeleteNotification", id);
            return NoContent();
        }


        [HttpPost("by-role")]
        [Authorize(Roles = "Admin,Staff,Owner")]
        public async Task<IActionResult> CreateByRole([FromBody] NotifyByRoleRequest request)
        {
            var result = await _service.CreateByRoleAsync(request);

            // Nếu có nhiều role
            foreach (var role in request.TargetRoles)
            {
                await _hubContext.Clients.Group(role.ToString())
                    .SendAsync("ReceiveRoleNotification", result);
            }

            return Ok(result);
        }


        [HttpPut("mark-read/{id}")]
        [Authorize(Roles = "Admin,Staff,Owner")]
        public async Task<IActionResult> MarkAsRead(Guid id)
        {
            var success = await _service.MarkAsReadAsync(id);
            await _hubContext.Clients.All.SendAsync("MarkAsRead", id);
            return success ? Ok("Marked as read") : NotFound();
        }

        [HttpPut("UpdateByrole{id}")]
        [Authorize(Roles = "Admin,Staff,Owner")]
        public async Task<IActionResult> UpdateByRole(Guid id, [FromBody] NotifyRequest request)
        {
            var result = await _service.UpdateAsync(id, request);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("all-by-user")]
        [Authorize] // Chỉ cần đăng nhập, không cần kiểm tra role
        public async Task<IActionResult> GetAllByUser()
        {
            var userId = GetUserIdFromToken();
            if (userId == Guid.Empty)
                return Unauthorized("UserId not found in token.");

            var result = await _service.GetAllByUserAsync(userId);
            return Ok(result);
        }

        [HttpPost("trigger-owner-register")]
        public async Task<IActionResult> TriggerOwnerRegister( [FromBody] TriggerOwnerRegisterRequest dto)
        {
            var userId = GetUserIdFromToken();
            if (dto == null)
                return BadRequest(new { message = "Missing AccountId information." });

            await _service.CreateNotifyForOwnerRegisterAsync(userId,dto);
            return Ok(new { message = "Notification has been created for Staff." });
        }

        [HttpPost("triger-aprove-Owner")]
        public async Task<IActionResult> TriggerAproveOwnerRegister([FromBody] TriggerOwnerRegisterRequest dto)
        {
            var userId = GetUserIdFromToken();
            if (dto == null)
                return BadRequest(new { message = "Missing AccountId information." });

            await _service.AproveNotifyForOwnerRegisterAsync(userId, dto);
            return Ok(new { message = "Notification has been created for Owner." });
        }

        [HttpPost("triger-reject-Owner")]
        public async Task<IActionResult> TriggerRejectOwnerRegister([FromBody] TriggerOwnerRegisterRequest dto)
        {
            var userId = GetUserIdFromToken();
            if (dto == null)
                return BadRequest(new { message = "Missing AccountId information." });

            await _service.RejectNotifyForOwnerRegisterAsync(userId, dto);
            return Ok(new { message = "Notification has been created for User." });
        }

        [HttpPost("schedule")]
        [Authorize(Roles = "Admin,Staff,Owner")]
        public async Task<IActionResult> ScheduleNotify([FromBody] NotifyByRoleRequest request)
        {
            if (request.ScheduledTime.HasValue && request.ScheduledTime.Value <= DateTime.UtcNow)
                return BadRequest(new { message = "Appointment time must be greater than current." });

            var userId = GetUserIdFromToken();
            await _service.CreateNotifyAsync(request, userId);
            return Ok(new { message = "Timer notification created successfully." });
        }


        [HttpGet("by-role")]
        [Authorize(Roles = "Admin,Staff,Owner,User")]
        public async Task<IActionResult> GetByRoleOrUser()
        {
            var userId = GetUserIdFromToken();

            // 🔍 Lấy role từ token
            var roleClaim = User.FindFirst(ClaimTypes.Role)?.Value;
            if (string.IsNullOrEmpty(roleClaim))
                return Unauthorized("Role not found in token.");

            // Ép kiểu role string → enum RoleEnum
            if (!Enum.TryParse(roleClaim, true, out RoleEnum role))
                return BadRequest($"Invalid role: {roleClaim}");

            var result = await _service.GetAllForRoleOrUserAsync(userId, role);
            return Ok(result);
        }

    }
}
