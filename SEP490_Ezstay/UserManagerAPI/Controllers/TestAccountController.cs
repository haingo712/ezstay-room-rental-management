using APIGateway.Helper.Interfaces;
using AuthApi.DTO.Request;
using AuthApi.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using UserManagerAPI.Service.Interfaces;

namespace UserManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
  
    public class TestAccountController : ControllerBase
    {
        private readonly IAccountApiClient _accountApi;

        private readonly IJwtClaimHelper _userClaimHelper;

        public TestAccountController(IAccountApiClient accountApi, IJwtClaimHelper userClaimHelper)
        {
            _accountApi = accountApi;
            _userClaimHelper = userClaimHelper;
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
        [Authorize(Roles = "Admin,Staff")]
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
          [Authorize(Roles = "Admin,Staff")]
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
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> Create([FromBody] AccountRequest request)
        {
            var token = GetToken();
            _accountApi.SetJwtToken(token);

            var created = await _accountApi.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = created!.Id }, created);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Staff")]
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

        [HttpPatch("{id}/ban")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> Ban(Guid id)
        {
            var token = GetToken();
            _accountApi.SetJwtToken(token);

            var account = await _accountApi.GetByIdAsync(id);
            var role = GetRoleFromToken(token);

            if (account == null || (role == RoleEnum.Staff && account.Role == RoleEnum.Admin))
                return Forbid();

            await _accountApi.BanAsync(id);
            return NoContent();
        }

        [HttpPatch("{id}/unban")]
          [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> Unban(Guid id)
        {
            var token = GetToken();
            _accountApi.SetJwtToken(token);

            var account = await _accountApi.GetByIdAsync(id);
            var role = GetRoleFromToken(token);

            if (account == null || (role == RoleEnum.Staff && account.Role == RoleEnum.Admin))
                return Forbid();

            await _accountApi.UnbanAsync(id);
            return NoContent();
        }
        [HttpPost("request-owner")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> RequestBecomeOwner([FromBody] SubmitOwnerRequestDto dto)
        {
            var accountId = _userClaimHelper.GetUserIdOrNull(User);
            if (accountId == null)
                return BadRequest(new { message = "Không tìm thấy thông tin tài khoản trong token." });

            // Gắn AccountId vào DTO trước khi gửi service
            dto.AccountId = accountId.Value;


            // Gọi client/service để tạo OwnerRequest, trả về DTO
            var resultDto = await _accountApi.SubmitOwnerRequestAsync(dto);

            if (resultDto == null)
                return BadRequest("Gửi đơn thất bại");

            return Ok(resultDto); // Trả về DTO đầy đủ
        }



    }
}
