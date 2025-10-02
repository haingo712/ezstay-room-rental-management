


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
    public IQueryable<ReviewReply> GetAllQueryable() => _replies.AsQueryable();

    public async Task<ReviewReply?> GetByIdAsync(Guid id)
        => await _replies.Find(x => x.Id == id).FirstOrDefaultAsync();

   
    public async Task<ReviewReply?> GetByReviewIdAsync(Guid reviewId)
    {
        return await _replies.Find(r => r.ReviewId == reviewId).FirstOrDefaultAsync();
    }

    public async Task AddAsync(ReviewReply reply)
    {
        await _replies.InsertOneAsync(reply);
    }

    public async Task UpdateAsync(ReviewReply reply)
    {
        await _replies.ReplaceOneAsync(r => r.Id == reply.Id, reply);
    }

    public async Task DeleteAsync(Guid replyId)
    {
        await _replies.DeleteOneAsync(r => r.Id == replyId);
    }
}
