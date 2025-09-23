using AmenityAPI.Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using AmenityAPI.Repository.Interface;


namespace AmenityAPI.Repository;

public class AmenityRepository:IAmenityRepository
{
    private readonly IMongoCollection<Amenity> _amenities;
    
    public AmenityRepository(IMongoDatabase database)
    {
        _amenities = database.GetCollection<Amenity>("Amenities");
    }
   

    public IQueryable<Amenity> GetAllAsQueryable()=> _amenities.AsQueryable();
    public async Task<IEnumerable<Amenity>> GetAll()
    {
        return await _amenities.Find(_ => true).ToListAsync();
    }
    
    // public async Task<IEnumerable<Amenity>> GetAll()
    // {
    //     return await _amenities.AsQueryable().ToListAsync();
    // }
    // public async Task<IEnumerable<Amenity>> GetAllByStaffId(Guid staffId)
    // {
    //     return await _amenities.Find(a=> a.StaffId == staffId).ToListAsync();
    // }
    
    public async Task<Amenity?> GetByIdAsync(Guid id)
    {
      return await _amenities.Find(a => a.Id == id).FirstOrDefaultAsync();
    }

    public async Task AddAsync(Amenity amenity)
    {
        await _amenities.InsertOneAsync(amenity);
    }
    // public async Task<bool> AmenityNameExistsAsync(string amenityNameExists)
    // {
    //     var filter = Builders<Amenity>.Filter.Regex(a => a.AmenityName, new MongoDB.Bson.BsonRegularExpression($"^{amenityNameExists}$", "i"));
    //     var result = await _amenities.Find(filter).AnyAsync();
    //     return result;
    // }
    
    public async Task<bool> AmenityNameExistsAsync(string amenityNameExists)
    {
         return await _amenities
        .AsQueryable()
        .AnyAsync(a => a.AmenityName.ToLower() == amenityNameExists.ToLower());
      // return await _amenities.Find(a => a.AmenityName.ToLower() == amenityNameExists.ToLower()).AnyAsync();
    }
    

    public async Task UpdateAsync(Amenity amenity)
    {
        await _amenities.ReplaceOneAsync(a => a.Id == amenity.Id, amenity);
    }
    public async Task DeleteAsync(Amenity amenity)
    {
        await _amenities.DeleteOneAsync(a => a.Id == amenity.Id);
    }

   
}