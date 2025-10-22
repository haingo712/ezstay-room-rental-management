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

    public async Task Add(ReviewReport report)
    {
        await _collection.InsertOneAsync(report);
    }

    public async Task<ReviewReport> GetById(Guid id)
    {
        return await _collection.Find(r => r.Id == id).FirstOrDefaultAsync();
    }

    public IQueryable<ReviewReport> GetAll() => _collection.AsQueryable();
    

    public async Task Update(ReviewReport report)
    {
        await _collection.ReplaceOneAsync(r => r.Id == report.Id, report);
    }
    public async Task Delete(ReviewReport report)
    {
        await _collection.DeleteOneAsync(r => r.Id == report.Id);
    }
}
