

using ElectricityReadingAPI.Model;

namespace ElectricityReadingAPI.Repository.Interface;

public interface IElectricityReadingRepository
{   
    IQueryable<ElectricityReading> GetAllOdata();
    // Task<IEnumerable<ElectricityReading>> GetAllByOwnerId(Guid ownerId);
    // Task<IEnumerable<ElectricityReading>> GetAllByUserId(Guid userId);
    Task<ElectricityReading?> GetByIdAsync(Guid id);
    Task AddAsync(ElectricityReading electricityReading);
    Task UpdateAsync(ElectricityReading electricityReading);
    Task DeleteAsync(ElectricityReading electricityReading);


}