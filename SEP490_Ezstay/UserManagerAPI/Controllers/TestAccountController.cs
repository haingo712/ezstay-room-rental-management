using AuthApi.DTO.Request;
using AuthApi.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
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



        private string GetToken()
        {
            var authHeader = HttpContext.Request.Headers["Authorization"].ToString();
            return authHeader.Replace("Bearer ", "");
        }

        private RoleEnum GetRoleFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            var roleClaim = jwt.Claims.FirstOrDefault(c =>
                c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value;

            return roleClaim switch
            {
                "Admin" => RoleEnum.Admin,
                "Staff" => RoleEnum.Staff,
                "Owner" => RoleEnum.Owner,
                _ => RoleEnum.User
            };
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var token = GetToken();
            _accountApi.SetJwtToken(token);

            var accounts = await _accountApi.GetAllAsync();

            var role = GetRoleFromToken(token);
            if (role == RoleEnum.Staff)
            {
                accounts = accounts.Where(a => a.Role != RoleEnum.Admin).ToList();
            }

            return Ok(accounts);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var token = GetToken();
            _accountApi.SetJwtToken(token);

            var account = await _accountApi.GetByIdAsync(id);

            var role = GetRoleFromToken(token);
            if (account == null || (role == RoleEnum.Staff && account.Role == RoleEnum.Admin))
                return Forbid();

            return Ok(account);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AccountRequest request)
        {
            var token = GetToken();
            _accountApi.SetJwtToken(token);

            var created = await _accountApi.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = created!.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] AccountRequest request)
        {
            var token = GetToken();
            _accountApi.SetJwtToken(token);

            var updated = await _accountApi.UpdateAsync(id, request);
            var role = GetRoleFromToken(token);

            if (updated == null || (role == RoleEnum.Staff && updated.Role == RoleEnum.Admin))
                return Forbid();

            return Ok(updated);
        }
    }
}
