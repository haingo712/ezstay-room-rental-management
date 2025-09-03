using RentalRequestAPI.Model;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using RentalRequestAPI.Repository.Interface;


namespace RentalRequestAPI.Repository;

public class RentalRequestRepository:IRentalRequestRepository
{
    private readonly IMongoCollection<RentalRequest> _amenities;
    
    public RentalRequestRepository(IMongoDatabase database)
    {
        _amenities = database.GetCollection<RentalRequest>("RentalRequests");
    }
    
    public IQueryable<RentalRequest> GetAllByOwnerIdOdata(Guid ownerId)=> _amenities.AsQueryable().Where(a=> a.OwnerId==ownerId);
    public IQueryable<RentalRequest> GetAllByUserIdOdata(Guid userId)=> _amenities.AsQueryable().Where(a=> a.UserId==userId);
    public async Task<IEnumerable<RentalRequest>> GetAllByOwnerId(Guid ownerId)
    {
        return await _amenities.Find(a=> a.OwnerId == ownerId).ToListAsync();
    }
    public async Task<IEnumerable<RentalRequest>> GetAllByUserId(Guid userId)
    {
        return await _amenities.Find(a=> a.UserId==userId).ToListAsync();
    }
    
    public async Task<RentalRequest?> GetByIdAsync(Guid id)
    {
      return await _amenities.Find(a => a.Id == id).FirstOrDefaultAsync();
    }

    public async Task AddAsync(RentalRequest rentalRequest)
    {
        await _amenities.InsertOneAsync(rentalRequest);
    }
    public async Task UpdateAsync(RentalRequest rentalRequest)
    {
        await _amenities.ReplaceOneAsync(a => a.Id == rentalRequest.Id, rentalRequest);
    }
    // public async Task<bool> AmenityNameExistsAsync(string amenityNameExists)
    // {
    //     var filter = Builders<Amenity>.Filter.Regex(a => a.AmenityName, new MongoDB.Bson.BsonRegularExpression($"^{amenityNameExists}$", "i"));
    //     var result = await _amenities.Find(filter).AnyAsync();
    //     return result;
    // }
    
    // public async Task<bool> AmenityNameExistsAsync(string amenityNameExists)
    // {
    //      return await _amenities
    //     .AsQueryable()
    //     .AnyAsync(a => a.AmenityName.ToLower() == amenityNameExists.ToLower());
    //   // return await _amenities.Find(a => a.AmenityName.ToLower() == amenityNameExists.ToLower()).AnyAsync();
    // }



  
    // public async Task DeleteAsync(RentalRequest rentalRequest)
    // {
    //     // var filter = Builders<Amenity>.Filter.Eq(a => a.AmenityId, amenity.AmenityId);
    //     // await _amenities.DeleteOneAsync(filter);
    //     await _amenities.DeleteOneAsync(a => a.Id == rentalRequest.Id);
    // }

   
}