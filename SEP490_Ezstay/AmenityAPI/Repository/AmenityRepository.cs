using AmenityAPI.Data;
using AmenityAPI.Models;
using AmenityAPI.Repository.Interface;
using Microsoft.EntityFrameworkCore;


namespace AmenityAPI.Repository;

public class AmenityRepository:IAmenityRepository
{
    private readonly AppDbContext _context;

    public AmenityRepository(AppDbContext context)
    {
        _context = context;
    }

    public IQueryable<Amenity> GetAllOdata()=> _context.Amenities.AsQueryable();
    
    public  async Task<IEnumerable<Amenity>> GetAll()=>await _context.Amenities.ToListAsync();

    public async Task<IEnumerable<String>> GetAllDistinctNameAsync()
    {
        var names = await _context.Amenities
            .Select(a => a.AmenityName)
            .Distinct()
            .ToListAsync();
        return names;
    }

    public async Task<Amenity?> GetByIdAsync(int id)
    {
      return await _context.Amenities.FindAsync(id);
    }

    public async Task AddAsync(Amenity amenity)
    {
       _context.Amenities.Add(amenity);
       await _context.SaveChangesAsync();
    }
    public async Task<bool> AmenityNameExistsAsync(string amenityNameExists)
    => await _context.Amenities
        .AnyAsync(r => r.AmenityName.ToLower() == amenityNameExists.ToLower());
    // public async Task<bool> RoomNameExistsInHouse(int houseId, string roomName)
    // {
    //     return await _context.Rooms
    //         .AnyAsync(r => r.HouseId == houseId && r.RoomName.ToLower() == roomName.ToLower());
    // }
    public async  Task UpdateAsync(Amenity amenity)
    {
        _context.Amenities.Update(amenity);
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteAsync(Amenity amenity)
    { 
            _context.Amenities.Remove(amenity);
            await _context.SaveChangesAsync();
    }
}