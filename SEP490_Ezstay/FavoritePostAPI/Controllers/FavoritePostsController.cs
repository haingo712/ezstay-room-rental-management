using FavoritePostAPI.DTO;
using FavoritePostAPI.DTO.Request;
using FavoritePostAPI.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FavoritePostAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FavoritePostsController : ControllerBase
    {
        private readonly IFavoritePostService _service;

        public FavoritePostsController(IFavoritePostService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> AddFavorite([FromBody] FavoritePostCreateDTO dto)
        {
            var result = await _service.AddFavoriteAsync(User, dto);
            return Ok(result);
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyFavorites()
        {
            var result = await _service.GetFavoritesByUserAsync(User);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveFavorite(Guid id)
        {
            var success = await _service.RemoveFavoriteAsync(User, id);
            return success ? NoContent() : NotFound();
        }
    }
}
