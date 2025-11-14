using AmenityAPI.Models;

namespace AmenityAPI.Repository.Interface;

public interface IAmenityRepository
{
    IQueryable<Amenity> GetAll();
    Task<Amenity> GetById(Guid id);
    
    Task Add(Amenity amenity);
    Task Update(Amenity amenity);
    Task Delete(Amenity amenity);
    Task<bool> AmenityNameExists(string amenityName);
    // Task<bool> AmenityNameExists(string amenityName, Guid id);



}