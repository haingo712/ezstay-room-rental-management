using MongoDB.Driver;
using ReviewResponseAPI.Model;
using ReviewResponseAPI.Repository.Interface;

namespace ReviewResponseAPI.Repository;

public class ReviewResponseRepository : IReviewResponseRepository
{
    private readonly IMongoCollection<ReviewResponse> _responses;

    public ReviewResponseRepository(IMongoDatabase database)
    {
        _responses = database.GetCollection<ReviewResponse>("ReviewResponses");
    }

    public IQueryable<ReviewResponse> GetAllAsQueryable() => _responses.AsQueryable();

    public async Task<IEnumerable<ReviewResponse>> GetAll()
        => await _responses.Find(_ => true).ToListAsync();

    public async Task<IEnumerable<ReviewResponse>> GetAllByOwnerId(Guid ownerId)
        => await _responses.Find(r => r.OwnerId == ownerId).ToListAsync();

    public async Task<IEnumerable<ReviewResponse>> GetAllByReviewId(Guid reviewId)
        => await _responses.Find(r => r.ReviewId == reviewId).ToListAsync();

    public async Task<ReviewResponse?> GetByIdAsync(Guid id)
        => await _responses.Find(r => r.Id == id).FirstOrDefaultAsync();

    public async Task AddAsync(ReviewResponse reviewResponse)
        => await _responses.InsertOneAsync(reviewResponse);

    public async Task UpdateAsync(ReviewResponse reviewResponse)
        => await _responses.ReplaceOneAsync(r => r.Id == reviewResponse.Id, reviewResponse);

    public async Task DeleteAsync(ReviewResponse reviewResponse)
        => await _responses.DeleteOneAsync(r => r.Id == reviewResponse.Id);
}