using AuthApi.DTO.Request;
using AuthApi.DTO.Response;
using AuthApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FaceLoginController : ControllerBase
    {
        private readonly IFaceLoginService _faceLoginService;
        private readonly ILogger<FaceLoginController> _logger;

        public FaceLoginController(
            IFaceLoginService faceLoginService,
            ILogger<FaceLoginController> logger)
        {
            _faceLoginService = faceLoginService;
            _logger = logger;
        }
        [HttpPost("login")]
        public async Task<ActionResult<FaceLoginResponseDto>> LoginWithFace([FromBody] FaceLoginRequestDto dto)
        {
            if (string.IsNullOrEmpty(dto.FaceImage))
            {
                return BadRequest(new FaceLoginResponseDto
                {
                    Success = false,
                    Message = "Face image is required."
                });
            }

            var result = await _faceLoginService.LoginWithFaceAsync(dto);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
        [HttpPost("register-face")]
        [Authorize]
        public async Task<ActionResult<RegisterFaceResponseDto>> RegisterFace([FromBody] RegisterFaceRequestDto dto)
        {
            var userId = GetUserIdFromClaims();
            if (userId == null)
            {
                return Unauthorized(new RegisterFaceResponseDto
                {
                    Success = false,
                    Message = "User not authenticated."
                });
            }

            if (string.IsNullOrEmpty(dto.FaceImage))
            {
                return BadRequest(new RegisterFaceResponseDto
                {
                    Success = false,
                    Message = "Face image is required."
                });
            }

            var result = await _faceLoginService.RegisterFaceAsync(userId.Value, dto);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
        [HttpPost("verify-face")]
        [Authorize]
        public async Task<ActionResult<VerifyFaceResponseDto>> VerifyFace([FromBody] VerifyFaceRequestDto dto)
        {
            var userId = GetUserIdFromClaims();
            if (userId == null)
            {
                return Unauthorized(new VerifyFaceResponseDto
                {
                    Success = false,
                    Message = "User not authenticated.",
                    IsMatch = false
                });
            }

            if (string.IsNullOrEmpty(dto.FaceImage))
            {
                return BadRequest(new VerifyFaceResponseDto
                {
                    Success = false,
                    Message = "Face image is required.",
                    IsMatch = false
                });
            }

            var result = await _faceLoginService.VerifyFaceAsync(userId.Value, dto);

            return Ok(result);
        }
        [HttpGet("my-faces")]
        [Authorize]
        public async Task<ActionResult<GetFacesResponseDto>> GetMyFaces()
        {
            var userId = GetUserIdFromClaims();
            if (userId == null)
            {
                return Unauthorized(new GetFacesResponseDto
                {
                    Success = false,
                    Message = "User not authenticated."
                });
            }

            var result = await _faceLoginService.GetFacesAsync(userId.Value);

            return Ok(result);
        }
        [HttpPut("update-face")]
        [Authorize]
        public async Task<ActionResult<UpdateFaceResponseDto>> UpdateFace([FromBody] UpdateFaceRequestDto dto)
        {
            var userId = GetUserIdFromClaims();
            if (userId == null)
            {
                return Unauthorized(new UpdateFaceResponseDto
                {
                    Success = false,
                    Message = "User not authenticated."
                });
            }

            var result = await _faceLoginService.UpdateFaceAsync(userId.Value, dto);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
        [HttpDelete("delete-face/{faceId}")]
        [Authorize]
        public async Task<ActionResult<DeleteFaceResponseDto>> DeleteFace(Guid faceId)
        {
            var userId = GetUserIdFromClaims();
            if (userId == null)
            {
                return Unauthorized(new DeleteFaceResponseDto
                {
                    Success = false,
                    Message = "User not authenticated."
                });
            }

            var result = await _faceLoginService.DeleteFaceAsync(userId.Value, faceId);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
        [HttpGet("has-face")]
        [Authorize]
        public async Task<ActionResult<object>> HasRegisteredFace()
        {
            var userId = GetUserIdFromClaims();
            if (userId == null)
            {
                return Unauthorized(new { Success = false, Message = "User not authenticated." });
            }

            var hasFace = await _faceLoginService.HasRegisteredFaceAsync(userId.Value);

            return Ok(new { Success = true, HasFace = hasFace });
        }

        #region Private Helper Methods

        private Guid? GetUserIdFromClaims()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return null;

            if (Guid.TryParse(userIdClaim, out var userId))
                return userId;

            return null;
        }

        #endregion
    }
}
