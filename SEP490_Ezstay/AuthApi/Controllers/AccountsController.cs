using AuthApi.DTO.Request;
using AuthApi.DTO.Response;
using AuthApi.Enums;
using AuthApi.Services;
using AuthApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _service;

        public AccountsController(IAccountService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var accounts = await _service.GetAllAsync();

            // Lấy role của user từ token
            var roleClaim = User.FindFirst(ClaimTypes.Role)?.Value;
            if (roleClaim == null) return Unauthorized();

            var role = Enum.Parse<RoleEnum>(roleClaim);

            // Staff không thấy Admin
            if (role == RoleEnum.Staff)
            {
                accounts = accounts.Where(a => a.Role != RoleEnum.Admin).ToList();
            }

            return Ok(accounts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var acc = await _service.GetByIdAsync(id);
            if (acc == null) return Forbid(); // Staff không được xem Admin
            return Ok(acc);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AccountRequest request)
        {
            var created = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] AccountRequest request)
        {
            var updated = await _service.UpdateAsync(id, request);
            if (updated == null) return Forbid(); // Staff không update Admin
            return Ok(updated);
        }

        [HttpPatch("{email}/verify")]
        public async Task<IActionResult> Verify(string email)
        {
            await _service.VerifyAsync(email);
            return NoContent();
        }

        [HttpPatch("{id}/ban")]
        public async Task<IActionResult> Ban(Guid id)
        {
            await _service.BanAsync(id);
            return NoContent();
        }

        [HttpPatch("{id}/unban")]
        public async Task<IActionResult> Unban(Guid id)
        {
            await _service.UnbanAsync(id);
            return NoContent();
        }


        [HttpPut("update-fullname/{id}")]
        public async Task<IActionResult> UpdateFullName(Guid id, [FromBody] string fullName)
        {
            var result = await _service.UpdateFullNameAsync(id, fullName);
            return result
                ? Ok("Cập nhật tên thành công")
                : NotFound("Không tìm thấy tài khoản");
        }

    }
}
