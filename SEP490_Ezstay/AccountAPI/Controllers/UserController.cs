using AccountAPI.DTO.Request;
using AccountAPI.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccountAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public UserController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost("create-profile")]
        public async Task<IActionResult> CreateProfile([FromBody] UserDTO userDto)
        {
            var userId = _tokenService.GetUserIdFromClaims(User);
            var success = await _userService.CreateProfileAsync(userId, userDto);
            return success ? Ok("Tạo profile thành công.") : BadRequest("Không tạo được profile.");
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = _tokenService.GetUserIdFromClaims(User);
            var profile = await _userService.GetProfileAsync(userId);

            if (profile == null)
                return NotFound("Không tìm thấy profile.");

            // 👉 Lấy thông tin từ token
            profile.FullName = _tokenService.GetFullNameFromClaims(User);
            profile.Phone = _tokenService.GetPhoneFromClaims(User);

            return Ok(profile);
        }

    }
}