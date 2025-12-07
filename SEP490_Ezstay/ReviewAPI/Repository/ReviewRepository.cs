using MongoDB.Driver;
using ReviewAPI.Model;
using ReviewAPI.Repository.Interface;

namespace ReviewAPI.Repository;

public class ReviewRepository : IReviewRepository
{
    private readonly IMongoCollection<Review> _reviews;

    public ReviewRepository(IMongoDatabase database)
    {
        _reviews = database.GetCollection<Review>("Reviews");
    }
    //public IQueryable<Review> GetAll() => _reviews.AsQueryable();
    public IQueryable<Review> GetAllByOwnerId(Guid ownerId) => _reviews.AsQueryable()
        .Where(r => r.OwnerId == ownerId);
    public Task<bool> ReviewExistsByContractId(Guid contractId)
    {
        return _reviews.Find(r => r.ContractId == contractId).AnyAsync();
    }
    public async Task<Review?> GetById(Guid id)
    {
        return await _reviews.Find(r => r.Id == id).FirstOrDefaultAsync();
    }

    public IQueryable<Review> GetAllReviewByRoomId(Guid roomId)
    {
      return  _reviews.AsQueryable()
            .Where(r => r.RoomId == roomId);
    }

    public async Task Add(Review review)
    {
        await _reviews.InsertOneAsync(review);
    }
    public async Task Update(Review review)
    {
        await _reviews.ReplaceOneAsync(r => r.Id == review.Id, review);
    }
}