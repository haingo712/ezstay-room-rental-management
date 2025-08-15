using AmenityAPI.Models;

namespace AmenityAPI.Repository.Interface;

public interface IAmenityRepository
{
    IQueryable<Amenity> GetAllOdata();
    Task<IEnumerable<Amenity>> GetAll();
    Task<IEnumerable<String>> GetAllDistinctNameAsync();
    Task<Amenity?> GetByIdAsync(int id);
    Task AddAsync(Amenity amenity);
    Task UpdateAsync(Amenity amenity);
    Task DeleteAsync(Amenity amenity);
    Task<bool> AmenityNameExistsAsync(string roomName);
   // Task<bool> RoomNameExistsInHouse(int houseId, string roomName);


}