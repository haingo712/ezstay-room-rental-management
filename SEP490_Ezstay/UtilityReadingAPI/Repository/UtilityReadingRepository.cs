using System.Text.RegularExpressions;
using UtilityReadingAPI.Data;
using UtilityReadingAPI.Model;
using UtilityReadingAPI.Repository.Interface;
using MongoDB.Driver;
using MongoDB.Driver.Linq;


namespace UtilityReadingAPI.Repository;

public class UtilityReadingRepository:IUtilityReadingRepository
{
    private readonly IMongoCollection<UtilityReading> _utilityReadings;
    
    public UtilityReadingRepository(MongoDbService service)
    {
        _utilityReadings=service.UtilityReadings;
    }
   

    public IQueryable<Model.UtilityReading> GetAllOdata()=> _utilityReadings.AsQueryable();

    public async Task<bool> ExistsUtilityReadingInMonthAsync(Guid roomId, string type, DateTime readingDate)
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
    // public async Task<IEnumerable<ElectricityReading>> GetAllByOwnerId(Guid ownerId)
    // {
    //     return await _electricityReadings.Find(a=> a.OwnerId == ownerId).ToListAsync();
    // }
    
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
        // var filter = Builders<Amenity>.Filter.Eq(a => a.AmenityId, amenity.AmenityId);
        // await _amenities.DeleteOneAsync(filter);
        await _utilityReadings.DeleteOneAsync(a => a.Id == utilityReading.Id);
    }

   
}