using AmenityAPI.Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using AmenityAPI.Repository.Interface;


namespace AmenityAPI.Repository;

public class AmenityRepository:IAmenityRepository
{
    private readonly IMongoCollection<Amenity> _collection;
    
    public AmenityRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<Amenity>("Amenities");
    }
   

    public IQueryable<Amenity> GetAll()=> _collection.AsQueryable();
    
    public async Task<Amenity> GetById(Guid id)
    {
      return await _collection.Find(a => a.Id == id).FirstOrDefaultAsync();
    }

    public async Task Add(Amenity amenity)
    {
        await _collection.InsertOneAsync(amenity);
    }
    // public async Task<bool> AmenityNameExistsAsync(string amenityNameExists)
    // {
    //     var filter = Builders<Amenity>.Filter.Regex(a => a.AmenityName, new MongoDB.Bson.BsonRegularExpression($"^{amenityNameExists}$", "i"));
    //     var result = await _amenities.Find(filter).AnyAsync();
    //     return result;
    // }
    
    public async Task<bool> AmenityNameExists(string amenityName)
    {
         return await _collection
        .AsQueryable()
        .AnyAsync(a => a.AmenityName.ToLower() == amenityName.ToLower());
      // return await _amenities.Find(a => a.AmenityName.ToLower() == amenityNameExists.ToLower()).AnyAsync();
    }
    // public async Task<bool> AmenityNameExists(string amenityName, Guid id)
    // {
    //     return await _collection
    //         .AsQueryable()
    //         .AnyAsync(a => 
    //             a.AmenityName.ToLower() == amenityName.ToLower() &&
    //             ( a.Id != id));
    //     
    // }

    public async Task Update(Amenity amenity)
    {
        await _collection.ReplaceOneAsync(a => a.Id == amenity.Id, amenity);
    }
    public async Task Delete(Amenity amenity)
    {
        await _collection.DeleteOneAsync(a => a.Id == amenity.Id);
    }

   
}