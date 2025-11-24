using ReviewAPI.Model;

namespace ReviewAPI.Repository.Interface;

public interface IReviewRepository
{
   // IQueryable<Review> GetAll();
    IQueryable<Review> GetAllByOwnerId(Guid ownerId);
    Task<bool> ReviewExistsByContractId(Guid contractId);
    Task<Review?> GetById(Guid id);
    Task Add(Review review);
    Task Update(Review review);
    Task<List<Review>> GetByRoomIds(List<Guid> roomIds);
}