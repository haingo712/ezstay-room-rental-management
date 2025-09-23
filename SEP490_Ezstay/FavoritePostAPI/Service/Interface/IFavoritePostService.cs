using FavoritePostAPI.DTO.Request;
using System.Security.Claims;


namespace FavoritePostAPI.Service
{
    public interface IFavoritePostService
    {
        Task<FavoritePostDTO> AddFavoriteAsync(ClaimsPrincipal user, FavoritePostCreateDTO dto);
        Task<IEnumerable<FavoritePostDTO>> GetFavoritesByUserAsync(ClaimsPrincipal user);
        Task<bool> RemoveFavoriteAsync(ClaimsPrincipal user, Guid favoriteId);
    }
}
