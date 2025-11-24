using MongoDB.Driver;
using ReviewAPI.Model;
using ReviewAPI.Repository.Interface;

namespace ReviewAPI.Repository;

public class ReviewReplyRepository : IReviewReplyRepository
{
    private readonly IMongoCollection<ReviewReply> _replies;

    public ReviewReplyRepository(IMongoDatabase database)
    {
        _replies = database.GetCollection<ReviewReply>("ReviewReplies");
    }
    public IQueryable<ReviewReply> GetAll() => _replies.AsQueryable();

    public async Task<ReviewReply?> GetById(Guid id)
        => await _replies.Find(x => x.Id == id).FirstOrDefaultAsync();

   
    public async Task<ReviewReply?> GetByReviewId(Guid reviewId)
    {
        return await _replies.Find(r => r.ReviewId == reviewId).FirstOrDefaultAsync();
    }

    public async Task Add(ReviewReply reviewReply)
    {
        await _replies.InsertOneAsync(reviewReply);
    }

    public async Task Update(ReviewReply reviewReply)
    {
        await _replies.ReplaceOneAsync(r => r.Id == reviewReply.Id, reviewReply);
    }

    public async Task Delete(Guid id)
    {
        await _replies.DeleteOneAsync(r => r.Id == id);
    }
}