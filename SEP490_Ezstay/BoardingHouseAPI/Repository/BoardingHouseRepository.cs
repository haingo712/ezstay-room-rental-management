using BoardingHouseAPI.Data;
using BoardingHouseAPI.Models;
using BoardingHouseAPI.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace BoardingHouseAPI.Repository
{
    public class BoardingHouseRepository : IBoardingHouseRepository
    {
        private readonly IMongoCollection<BoardingHouse> _houses;
        private readonly IMongoCollection<HouseLocation> _locations;

        public BoardingHouseRepository(MongoDbService service)
        {
            _houses = service.BoardingHouses;
            _locations = service.HouseLocations;
        }

        public IQueryable<BoardingHouse> GetAll()
        {
            return _houses.AsQueryable();
        }

        public IQueryable<BoardingHouse> GetBoardingHousesByOwnerId(Guid ownerId)
        {
            return _houses.AsQueryable().Where(h => h.OwnerId == ownerId);
        }

        public async Task<BoardingHouse?> GetByIdAsync(Guid id)
        {
            return await _houses.Find(h => h.Id == id).FirstOrDefaultAsync();
        }

        public async Task AddAsync(BoardingHouse house)
        {
            if (house.Location != null)
            {
                house.Location.Id = Guid.NewGuid();
                await _locations.InsertOneAsync(house.Location);
            }
            house.HouseLocationId = house.Location?.Id ?? Guid.Empty;
            await _houses.InsertOneAsync(house);
        }

        public async Task UpdateAsync(BoardingHouse house)
        {
            await _houses.ReplaceOneAsync(h => h.Id == house.Id, house);
            if (house.Location != null)
            {
                await _locations.ReplaceOneAsync(l => l.Id == house.Location.Id, house.Location);
            }
        }

        public async Task DeleteAsync(BoardingHouse house)
        {
            await _houses.DeleteOneAsync(h => h.Id == house.Id);
            await _locations.DeleteOneAsync(l => l.Id == house.HouseLocationId);
        }

        // Check if a house location exists with its house name and full address
        public Task<bool> LocationExistsWithHouseName(string houseName, string address)
        {
            return Task.FromResult(_houses.AsQueryable()
                .Any(h => h.HouseName == houseName && h.Location != null && h.Location.FullAddress == address));
        }

        // Check if a house location exists with its full address
        public Task<bool> LocationExists(string address)
        {
            return Task.FromResult(_locations.AsQueryable()
                .Any(l => l.FullAddress == address));
        }
    }
}
