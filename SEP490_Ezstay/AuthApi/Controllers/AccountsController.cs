using AuthApi.DTO.Request;
using AuthApi.DTO.Response;
using AuthApi.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost]
        public async Task<ActionResult<AccountResponse>> Create([FromBody] AccountRequest request)
        {
            var result = await _service.CreateAsync(request);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AccountResponse>> GetById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<List<AccountResponse>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AccountResponse>> Update(Guid id, [FromBody] AccountRequest request)
        {
            var result = await _service.UpdateAsync(id, request);
            if (result == null) return NotFound();
            return Ok(result);
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

    }
}
