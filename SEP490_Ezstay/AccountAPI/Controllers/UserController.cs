using AccountAPI.DTO.Request;
using AccountAPI.DTO.Response;
using AccountAPI.DTO.Resquest;
using AccountAPI.Service.Interfaces;
using APIGateway.Helper.Interfaces;
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
        private readonly IUserClaimHelper _userClaimHelper;

        public UserController(IUserService userService, IUserClaimHelper userClaimHelper)
        {
            _userService = userService;
            _userClaimHelper = userClaimHelper;
        }

        [HttpPost("create-profile")]
        public async Task<IActionResult> CreateProfile([FromBody] UserDTO userDto)
        {
            var userId = _userClaimHelper.GetUserId(User);
            var success = await _userService.CreateProfileAsync(userId, userDto);
            return success ? Ok("Tạo profile thành công.") : BadRequest("Không tạo được profile.");
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = _userClaimHelper.GetUserId(User);
            var profile = await _userService.GetProfileAsync(userId);

            if (profile == null)
                return NotFound("Không tìm thấy profile.");

            // 👉 Lấy thông tin từ token
            profile.FullName = _userClaimHelper.GetFullName(User);
            profile.Phone = _userClaimHelper.GetPhone(User);

            return Ok(profile);
        }

        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateProfile([FromForm] UpdateUserDTO dto)
        {
            var userId = _userClaimHelper.GetUserId(User);
            var success = await _userService.UpdateProfileAsync(userId, dto, User); // 👈 Truyền ClaimsPrincipal vào

            return success
                ? Ok(ApiResponse<string>.Ok(null, "Cập nhật profile thành công."))
                : BadRequest(ApiResponse<string>.Fail("Cập nhật thất bại."));
        }


    }
}
