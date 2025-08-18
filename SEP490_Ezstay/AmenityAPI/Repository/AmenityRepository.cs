using System.Text.RegularExpressions;
using AmenityAPI.Data;
using AmenityAPI.Models;
using AmenityAPI.Repository.Interface;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;


namespace AmenityAPI.Repository;

public class AmenityRepository:IAmenityRepository
{
    private readonly IMongoCollection<Amenity> _amenities;
    
    public AmenityRepository(MongoDbService service)
    {
        _amenities = service.Amenities;
    }
   

    public IQueryable<Amenity> GetAllOdata()=> _amenities.AsQueryable();

  
    public async Task<IEnumerable<Amenity>> GetAllByOwnerId(Guid ownerId)
    {
        return await _amenities.Find(a=> a.OwnerId == ownerId).ToListAsync();
    }
    
    public async Task<IEnumerable<string>> GetAllDistinctNameAsync()
    {
        return await _amenities.Distinct<string>("AmenityName", 
            FilterDefinition<Amenity>.Empty).ToListAsync();
    }
   

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
    }



    public async Task UpdateAsync(Amenity amenity)
    {
        // var filter = Builders<Amenity>.Filter.Eq(a => a.AmenityId, amenity.AmenityId);
        // await _amenities.ReplaceOneAsync(filter, amenity);
        await _amenities.ReplaceOneAsync(a => a.Id == amenity.Id, amenity);
    }
    public async Task DeleteAsync(Amenity amenity)
    {
        // var filter = Builders<Amenity>.Filter.Eq(a => a.AmenityId, amenity.AmenityId);
        // await _amenities.DeleteOneAsync(filter);
        await _amenities.DeleteOneAsync(a => a.Id == amenity.Id);
    }

   
}