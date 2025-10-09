using UtilityReadingAPI.Model;
using UtilityReadingAPI.Repository.Interface;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Shared.Enums;


namespace UtilityReadingAPI.Repository;

public class UtilityReadingRepository:IUtilityReadingRepository
{
    private readonly IMongoCollection<UtilityReading> _utilityReadings;
    
    public UtilityReadingRepository(IMongoDatabase database)
    {
        _utilityReadings=database.GetCollection<UtilityReading>("UtilityReadings");
    }

    public IQueryable<UtilityReading> GetAllAsQueryable()=> _utilityReadings.AsQueryable();

    public async Task<bool> ExistsUtilityReadingInMonthAsync(Guid roomId, UtilityType type, DateTime readingDate)
    {
        var startOfMonth = new DateTime(readingDate.Year, readingDate.Month, 1);
        var endOfMonth = startOfMonth.AddMonths(1).AddTicks(-1); 
//var endOfMonth = new DateTime(readingDate.Year, readingDate.Month, DateTime.DaysInMonth(readingDate.Year, readingDate.Month), 23, 59, 59, 999);
        var filter = Builders<UtilityReading>.Filter.And(
            Builders<UtilityReading>.Filter.Eq(r => r.RoomId, roomId),
            Builders<UtilityReading>.Filter.Eq(r => r.Type, type),
            Builders<UtilityReading>.Filter.Gte(r => r.ReadingDate, startOfMonth),
            Builders<UtilityReading>.Filter.Lte(r => r.ReadingDate, endOfMonth)
        );
        return await _utilityReadings.Find(filter).AnyAsync();
    }
    
    public async Task<UtilityReading?> GetByIdAsync(Guid id)
    {
      return await _utilityReadings.Find(a => a.Id == id).FirstOrDefaultAsync();
    }

    public async Task AddAsync(UtilityReading utilityReading)
    {
        await _utilityReadings.InsertOneAsync(utilityReading);
    }
   
    
    public async Task UpdateAsync(UtilityReading utilityReading)
    {
        await _utilityReadings.ReplaceOneAsync(a => a.Id == utilityReading.Id, utilityReading);
    }
    public async Task DeleteAsync(UtilityReading utilityReading)
    {
        await _utilityReadings.DeleteOneAsync(a => a.Id == utilityReading.Id);
    }

   
}