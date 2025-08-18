using AmenityAPI.Models;

namespace AmenityAPI.Repository.Interface;

public interface IAmenityRepository
{
    IQueryable<Amenity> GetAllOdata();
    Task<IEnumerable<Amenity>> GetAllByOwnerId(Guid ownerId);
    Task<IEnumerable<String>> GetAllDistinctNameAsync();
    Task<Amenity?> GetByIdAsync(Guid id);
    Task AddAsync(Amenity amenity);
    Task UpdateAsync(Amenity amenity);
    Task DeleteAsync(Amenity amenity);
    Task<bool> AmenityNameExistsAsync(string roomName);


}