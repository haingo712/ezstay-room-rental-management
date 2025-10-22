using ReviewReportAPI.Model;

namespace ReviewReportAPI.Repository.Interface;

public interface IReviewReportRepository
{
    Task Add(ReviewReport report);
    Task<ReviewReport> GetById(Guid id);
    IQueryable<ReviewReport> GetAll();
   
    Task Update(ReviewReport report);
    Task Delete(ReviewReport report);
}
