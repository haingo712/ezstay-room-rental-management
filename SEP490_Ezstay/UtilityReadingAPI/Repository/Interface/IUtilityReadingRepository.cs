

using Shared.Enums;
using UtilityReadingAPI.Model;

namespace UtilityReadingAPI.Repository.Interface;

public interface IUtilityReadingRepository
{   
    Task<bool> ExistsUtilityReadingInMonthAsync(Guid contractId, UtilityType type,  DateTime readingDate);
    
    IQueryable<UtilityReading> GetAll();
    IQueryable<UtilityReading> GetAllByContractId(Guid contractId);
    Task<UtilityReading?> GetLatestReading(Guid contractId, UtilityType type, int month, int year);
    Task<UtilityReading> GetFirstReading(Guid contractId, UtilityType type);
    Task<UtilityReading> GetByIdAsync(Guid id);
    Task AddAsync(UtilityReading utilityReading);
    Task UpdateAsync(UtilityReading utilityReading);
    Task DeleteAsync(UtilityReading utilityReading);


}