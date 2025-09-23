using FavoritePostAPI.Models;


namespace FavoritePostAPI.Repository.Interface
{
    public interface IFavoritePostRepository
    {
        Task<FavoritePost?> GetByAccountAndPostAsyn(Guid accountId, Guid postId);
        Task<FavoritePost> CreateAsync(FavoritePost entity);
        Task<IEnumerable<FavoritePost>> GetByAccountAsync(Guid accountId);
        Task<FavoritePost?> GetByIdAsync(Guid id);
        Task<bool> DeleteAsyn(Guid id);
    }
}
