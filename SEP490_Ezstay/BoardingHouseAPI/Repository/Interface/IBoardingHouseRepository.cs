using BoardingHouseAPI.Models;

namespace BoardingHouseAPI.Repository.Interface
{
    public interface IBoardingHouseRepository
    {        
        IQueryable<BoardingHouse> GetAll();
        IQueryable<BoardingHouse> GetBoardingHousesByOwnerId(Guid ownerId);
        Task<BoardingHouse?> GetByIdAsync(Guid id);
        Task AddAsync(BoardingHouse house);
        Task UpdateAsync(BoardingHouse house);
        Task DeleteAsync(BoardingHouse house);
        Task<bool> BoardingHouseExistsByOwnerAndNameAsync(Guid ownerId, string houseName);
    }
}
