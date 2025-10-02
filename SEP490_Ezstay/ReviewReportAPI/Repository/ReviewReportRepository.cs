using MongoDB.Driver;
using ReviewReportAPI.Model;
using ReviewReportAPI.Repository.Interface;

namespace ReviewReportAPI.Repository;

public class ReviewReportRepository : IReviewReportRepository
{
    private readonly IMongoCollection<ReviewReport> _collection;

    public ReviewReportRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<ReviewReport>("ReviewReports");
    }

    public async Task AddAsync(ReviewReport report)
    {
        await _collection.InsertOneAsync(report);
    }

    public async Task<ReviewReport?> GetByIdAsync(Guid id)
    {
        return await _collection.Find(r => r.Id == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<ReviewReport>> GetAllAsync()
    {
        return await _collection.Find(_ => true).ToListAsync();
    }

    public async Task UpdateAsync(ReviewReport report)
    {
        await _collection.ReplaceOneAsync(r => r.Id == report.Id, report);
    }
}
