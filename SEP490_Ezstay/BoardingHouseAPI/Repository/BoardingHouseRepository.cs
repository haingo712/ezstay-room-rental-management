using BoardingHouseAPI.Data;
using BoardingHouseAPI.DTO;
using BoardingHouseAPI.Models;
using BoardingHouseAPI.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace BoardingHouseAPI.Repository
{
    public class BoardingHouseRepository : IBoardingHouseRepository
    {
        private readonly IMongoCollection<BoardingHouse> _houses;

        public BoardingHouseRepository(MongoDbService service)
        {
            _houses = service.BoardingHouses;
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
            await _houses.InsertOneAsync(house);
        }

        public async Task UpdateAsync(BoardingHouse house)
        {
           await _houses.ReplaceOneAsync(h => h.Id == house.Id, house);
        }

        public async Task DeleteAsync(BoardingHouse house)
        {
            await _houses.DeleteOneAsync(h => h.Id == house.Id);
        }

        public async Task<bool> BoardingHouseExistsByOwnerAndNameAsync(Guid ownerId, string houseName)
        {
            return await _houses
                .Find(h => h.OwnerId == ownerId && h.HouseName == houseName)
                .AnyAsync();
        }
    }
}
