namespace AggregatorAPI.Services.Interfaces;

public interface IUserService
{
    Task<object?> GetUserAsync(Guid userId);
}