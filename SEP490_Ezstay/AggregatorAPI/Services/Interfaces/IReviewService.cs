namespace AggregatorAPI.Services.Interfaces;

public interface IReviewService
{
    Task<object?> GetReviewAsync(Guid reviewId);
}