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

    public IQueryable<Review> GetAll() => _reviews.AsQueryable();
    public Task<bool> ReviewExistsByContractId(Guid contractId)
    {
        return _reviews.Find(r => r.ContractId == contractId).AnyAsync();
    }

    // public async Task<Review?> GetByContractId(Guid contractId)
    //     => await _reviews.Find(x => x.ContractId == contractId).FirstOrDefaultAsync();
    //
    public async Task<Review?> GetById(Guid id)
    {
        return await _reviews.Find(r => r.Id == id).FirstOrDefaultAsync();
    }
    public async Task<List<Review>> GetByRoomIds(List<Guid> roomIds)
    {
        return await _reviews
            .Find(r => roomIds.Contains(r.RoomId))
            .ToListAsync();
    }
    public async Task Add(Review review)
    {
        await _reviews.InsertOneAsync(review);
    }

    public async Task Update(Review review)
    {
        await _reviews.ReplaceOneAsync(r => r.Id == review.Id, review);
    }

    public async Task Delete(Review review)
    {
        await _reviews.DeleteOneAsync(r => r.Id == review.Id);
    }
}