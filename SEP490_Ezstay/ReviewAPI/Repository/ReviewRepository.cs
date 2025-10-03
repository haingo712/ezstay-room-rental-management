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

    public IQueryable<Review> GetAllAsQueryable() => _reviews.AsQueryable();

    public async Task<IEnumerable<Review>> GetAll()
    {
        return await _reviews.Find(_ => true).ToListAsync();
    }
    public async Task<Review?> GetByContractIdAsync(Guid contractId)
        => await _reviews.Find(x => x.ContractId == contractId).FirstOrDefaultAsync();


    public async Task<IEnumerable<Review>> GetAllByPostId(Guid postId)
    {
        return await _reviews.Find(r => r.PostId == postId).ToListAsync();
    }

    public async Task<Review?> GetByIdAsync(Guid id)
    {
        return await _reviews.Find(r => r.Id == id).FirstOrDefaultAsync();
    }

    public async Task AddAsync(Review review)
    {
        await _reviews.InsertOneAsync(review);
    }

    public async Task UpdateAsync(Review review)
    {
        await _reviews.ReplaceOneAsync(r => r.Id == review.Id, review);
    }

    public async Task DeleteAsync(Review review)
    {
        await _reviews.DeleteOneAsync(r => r.Id == review.Id);
    }
}