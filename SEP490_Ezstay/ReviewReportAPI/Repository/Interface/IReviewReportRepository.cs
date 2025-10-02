using ReviewReportAPI.Model;

namespace ReviewReportAPI.Repository.Interface;

public interface IReviewReportRepository
{
    Task AddAsync(ReviewReport report);
    Task<ReviewReport?> GetByIdAsync(Guid id);
    Task<IEnumerable<ReviewReport>> GetAllAsync();
    Task UpdateAsync(ReviewReport report);
}
