

using UtilityReadingAPI.Enum;
using UtilityReadingAPI.Model;

namespace UtilityReadingAPI.Repository.Interface;

public interface IUtilityReadingRepository
{   
    Task<bool> ExistsUtilityReadingInMonthAsync(Guid roomId, UtilityType type,  DateTime readingDate);
    IQueryable<UtilityReading> GetAllAsQueryable();
    
    Task<UtilityReading?> GetByIdAsync(Guid id);
    Task AddAsync(UtilityReading utilityReading);
    Task UpdateAsync(UtilityReading utilityReading);
    Task DeleteAsync(UtilityReading utilityReading);


}