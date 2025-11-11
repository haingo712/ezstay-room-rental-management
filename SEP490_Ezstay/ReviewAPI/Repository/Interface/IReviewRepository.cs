using ReviewAPI.Model;

namespace ReviewAPI.Repository.Interface;

public interface IReviewRepository
{
    IQueryable<Review> GetAll();
    Task<bool> ReviewExistsByContractId(Guid contractId);
  //  Task<Review?> GetByContractId(Guid contractId);
    Task<Review?> GetById(Guid id);
    Task Add(Review review);
    Task Update(Review review);
    Task Delete(Review review);
    Task<List<Review>> GetByRoomIds(List<Guid> roomIds);
}