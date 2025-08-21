

using UtilityReadingAPI.Model;

namespace UtilityReadingAPI.Repository.Interface;

public interface IUtilityReadingRepository
{   
    Task<bool> ExistsUtilityReadingInMonthAsync(Guid roomId, string type,  DateTime readingDate);
    IQueryable<UtilityReading> GetAllOdata();
    // Task<IEnumerable<ElectricityReading>> GetAllByOwnerId(Guid ownerId);
    // Task<IEnumerable<ElectricityReading>> GetAllByUserId(Guid userId);
    Task<UtilityReading?> GetByIdAsync(Guid id);
    Task AddAsync(UtilityReading utilityReading);
    Task UpdateAsync(UtilityReading utilityReading);
    Task DeleteAsync(UtilityReading utilityReading);


}