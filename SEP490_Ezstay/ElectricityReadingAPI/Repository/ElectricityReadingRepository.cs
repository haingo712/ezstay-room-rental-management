using System.Text.RegularExpressions;
using ElectricityReadingAPI.Data;
using ElectricityReadingAPI.Model;
using ElectricityReadingAPI.Repository.Interface;
using MongoDB.Driver;
using MongoDB.Driver.Linq;


namespace ElectricityReadingAPI.Repository;

public class ElectricityReadingRepository:IElectricityReadingRepository
{
    private readonly IMongoCollection<ElectricityReading> _electricityReadings;
    
    public ElectricityReadingRepository(MongoDbService service)
    {
        _electricityReadings=service.ElectricityReadings;
    }
   

    public IQueryable<ElectricityReading> GetAllOdata()=> _electricityReadings.AsQueryable();

  
    // public async Task<IEnumerable<ElectricityReading>> GetAllByOwnerId(Guid ownerId)
    // {
    //     return await _electricityReadings.Find(a=> a.OwnerId == ownerId).ToListAsync();
    // }
    
   

    public async Task<ElectricityReading?> GetByIdAsync(Guid id)
    {
      return await _electricityReadings.Find(a => a.Id == id).FirstOrDefaultAsync();
    }

    public async Task AddAsync(ElectricityReading electricityReading)
    {
        await _electricityReadings.InsertOneAsync(electricityReading);
    }
   
    
    public async Task UpdateAsync(ElectricityReading electricityReading)
    {
      
        await _electricityReadings.ReplaceOneAsync(a => a.Id == electricityReading.Id, electricityReading);
    }
    public async Task DeleteAsync(ElectricityReading electricityReading)
    {
        // var filter = Builders<Amenity>.Filter.Eq(a => a.AmenityId, amenity.AmenityId);
        // await _amenities.DeleteOneAsync(filter);
        await _electricityReadings.DeleteOneAsync(a => a.Id == electricityReading.Id);
    }

   
}