using AccountAPI.DTO.Request;
using AccountAPI.DTO.Response;
using AccountAPI.DTO.Resquest;
using AccountAPI.Service;
using AccountAPI.Service.Interfaces;
using APIGateway.Helper.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccountAPI.Controllers
{
    [Route("api/User")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserClaimHelper _userClaimHelper;
        private readonly IAuthApiClient _authApiClient;

        public AccountController(IUserService userService, IUserClaimHelper userClaimHelper, IAuthApiClient authApiClient)
        {
            _userService = userService;
            _userClaimHelper = userClaimHelper;
            _authApiClient = authApiClient;
            _authApiClient = authApiClient;
        }


        [HttpPost("create-profile")]
        [Authorize(Roles = "User,Owner,Staff")]
        public async Task<IActionResult> CreateProfile([FromForm] UserDTO userDto)
        {
            var userId = _userClaimHelper.GetUserId(User);

            // Truyền thêm User (ClaimsPrincipal) vào service
            var success = await _userService.CreateProfileAsync(userId, userDto);

            return success
                ? Ok("Tạo profile thành công.")
                : BadRequest("Không tạo được profile.");
        }


        [HttpGet("profile")]
        [Authorize(Roles = "User,Owner,Staff")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = _userClaimHelper.GetUserId(User);
            var profile = await _userService.GetProfileAsync(userId);

            if (profile == null)
                return NotFound("Không tìm thấy profile.");

          

            return Ok(profile);
        }


        [HttpGet("searchphone/{phone}")]
        public async Task<IActionResult> GetPhone(string phone)
        {
            var profile = await _userService.GetPhone(phone);
            return Ok(profile);
        }
        [HttpGet("search-cccd/{citizenIdNumber}")]
        public async Task<IActionResult> GetCitizenIdNumber(string citizenIdNumber)
        {
            var profile = await _userService. GetCitizenIdNumber(citizenIdNumber);
            return Ok(profile);
        }

        [HttpPut("update-phone")]
        [Authorize(Roles = "User,Owner,Staff")]
        public async Task<IActionResult> UpdatePhone([FromBody] UpdatePhoneRequestDto dto)
        {
            var userId = _userClaimHelper.GetUserId(User);

            // ✅ Bước 1: xác thực OTP
            var isValidOtp = await _userService.VerifyPhoneOtpAsync(dto.Phone, dto.Otp);
            if (!isValidOtp)
                return BadRequest(ApiResponse<string>.Fail("OTP sai hoặc hết hạn"));

            // ✅ Bước 2: cập nhật số điện thoại
            var updated = await _userService.UpdatePhoneAsync(userId, dto.Phone);
            return updated
                ? Ok(ApiResponse<string>.Ok(null, "Cập nhật số điện thoại thành công"))
                : BadRequest(ApiResponse<string>.Fail("Không cập nhật được số điện thoại"));
        }

        [HttpPut("update-profile")]
        [Authorize(Roles = "User,Owner,Staff")]
        public async Task<IActionResult> UpdateProfile([FromForm] UpdateUserDTO dto)
        {
            var userId = _userClaimHelper.GetUserId(User);
            var updated = await _userService.UpdateProfile(userId, dto);

            return updated
                ? Ok(ApiResponse<string>.Ok(null, "Cập nhật thông tin thành công"))
                : BadRequest(ApiResponse<string>.Fail("Cập nhật thất bại"));
        }


        [HttpPut("update-email")]
        [Authorize(Roles = "User,Owner,Staff")]
        public async Task<IActionResult> UpdateEmail([FromBody] UpdateEmailRequestDto dto)
        {
            var currentEmail = _userClaimHelper.GetEmail(User);
            if (string.IsNullOrEmpty(currentEmail))
                return Unauthorized(ApiResponse<string>.Fail("Không tìm thấy email người dùng"));

            var result = await _userService.UpdateEmailAsync(currentEmail, dto.NewEmail, dto.Otp);
            return result
                ? Ok(ApiResponse<string>.Ok(null, "Cập nhật email thành công"))
                : BadRequest(ApiResponse<string>.Fail("Cập nhật email thất bại"));
        }

        [HttpPut("change-password")]
        [Authorize(Roles = "User,Owner,Staff")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest dto)
        {
            // ✅ Lấy userId từ token
            var userId = _userClaimHelper.GetUserId(User);

            // ✅ Gọi API lấy account để lấy Email
            var account = await _authApiClient.GetByIdAsync(userId);
            if (account == null || string.IsNullOrEmpty(account.Email))
            {
                return Unauthorized(ApiResponse<string>.Fail("Không tìm thấy thông tin tài khoản."));
            }

            // ✅ Gắn Email vào DTO để gửi qua AuthAPI
            dto.Email = account.Email;

            // ✅ Gọi API đổi mật khẩu
            var message = await _authApiClient.ChangePasswordAsync(dto);

            return message == "Đổi mật khẩu thành công."
                ? Ok(ApiResponse<string>.Ok(null, message))
                : BadRequest(ApiResponse<string>.Fail(message));
        }
    }
}
