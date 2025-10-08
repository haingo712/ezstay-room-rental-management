using AmenityAPI.Models;

namespace AmenityAPI.Repository.Interface;

public interface IAmenityRepository
{
    IQueryable<Amenity> GetAllAsQueryable();
    Task<IEnumerable<Amenity>> GetAll();
  //  Task<IEnumerable<Amenity>> GetAllByStaffId(Guid staffId);
    Task<Amenity?> GetById(Guid id);
    
    Task Add(Amenity amenity);
    Task Update(Amenity amenity);
    Task Delete(Amenity amenity);
    Task<bool> AmenityNameExists(string amenityName);
    Task<bool> AmenityNameExists(string amenityName, Guid id);



}