using AmenityAPI.Models;

namespace AmenityAPI.Repository.Interface;

public interface IAmenityRepository
{
    IQueryable<Amenity> GetAllOdata();
    Task<IEnumerable<Amenity>> GetAll();
    Task<IEnumerable<Amenity>> GetAllByStaffId(Guid staffId);
    Task<Amenity?> GetByIdAsync(Guid id);
    Task AddAsync(Amenity amenity);
    Task UpdateAsync(Amenity amenity);
    Task DeleteAsync(Amenity amenity);
    Task<bool> AmenityNameExistsAsync(string roomName);


}