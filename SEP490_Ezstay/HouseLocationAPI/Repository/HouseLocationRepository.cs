using HouseLocationAPI.Data;
using HouseLocationAPI.Models;
using HouseLocationAPI.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace HouseLocationAPI.Repository
{
    public class HouseLocationRepository : IHouseLocationRepository
    {
        private readonly IMongoCollection<HouseLocation> _houseLocations;

        public HouseLocationRepository(MongoDbService service)
        {
            _houseLocations = service.HouseLocations;
        }
      
        public IQueryable<HouseLocation> GetAll()
        {
            return _houseLocations.AsQueryable();
        }

        public IQueryable<HouseLocation> GetHouseLocationsByHouseId(Guid houseId)
        {
            return _houseLocations.AsQueryable().Where(hl => hl.HouseId == houseId);
        }

        public async Task<HouseLocation?> GetByIdAsync(Guid id)
        {
            return await _houseLocations.Find(h => h.Id == id).FirstOrDefaultAsync();
        }   

        public async Task AddAsync(HouseLocation hloc)
        {
           await _houseLocations.InsertOneAsync(hloc);
        }

        public async Task UpdateAsync(HouseLocation hloc)
        {
            await _houseLocations.ReplaceOneAsync(h => h.Id == hloc.Id, hloc);
        }

        public async Task DeleteAsync(HouseLocation hloc)
        {
            await _houseLocations.DeleteOneAsync(h => h.Id == hloc.Id);
        }

        public Task<bool> LocationExistsWithHouseIdAsync(Guid houseId, string address)
        {
            return _houseLocations
                .Find(hl => hl.HouseId == houseId && hl.FullAddress == address)
                .AnyAsync();
        }
    }
}
