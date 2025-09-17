using AuthApi.DTO.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManagerAPI.Service.Interfaces;

namespace UserManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Staff")]
    public class TestAccountController : ControllerBase
    {
        private readonly IAccountApiClient _accountApi;

        public TestAccountController(IAccountApiClient accountApi)
        {
            _accountApi = accountApi;
        }

        // GET: api/TestAccount
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var accounts = await _accountApi.GetAllAsync();
            return Ok(accounts);
        }

        // GET: api/TestAccount/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var account = await _accountApi.GetByIdAsync(id);
            if (account == null) return NotFound("Account not found");
            return Ok(account);
        }

        // POST: api/TestAccount
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AccountRequest request)
        {
            var created = await _accountApi.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = created!.Id }, created);
        }

        // PUT: api/TestAccount/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] AccountRequest request)
        {
            var updated = await _accountApi.UpdateAsync(id, request);
            if (updated == null) return NotFound("Account not found");
            return Ok(updated);
        }

        // PATCH: api/TestAccount/{id}/ban
        [HttpPatch("{id}/ban")]
        public async Task<IActionResult> Ban(Guid id)
        {
            await _accountApi.BanAsync(id);
            return NoContent();
        }

        // PATCH: api/TestAccount/{id}/unban
        [HttpPatch("{id}/unban")]
        public async Task<IActionResult> Unban(Guid id)
        {
            await _accountApi.UnbanAsync(id);
            return NoContent();
        }
    }
}
