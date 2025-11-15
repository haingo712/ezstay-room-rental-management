using MongoDB.Driver;
using SupportAPI.Model;
using SupportAPI.Repositories.Interfaces;


namespace SupportAPI.Repositories
{
    public class SupportRepository : ISuportRepository
    {
        private readonly IMongoCollection<Support> _supports;

        public SupportRepository(IMongoDatabase database)
        {
            _supports = database.GetCollection<Support>("SupportModel");
        }

        public async Task<List<Support>> GetAllAsync() =>
            await _supports.Find(_ => true).ToListAsync();

        public async Task<Support> GetByIdAsync(Guid id) =>
            await _supports.Find(s => s.id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Support support) =>
            await _supports.InsertOneAsync(support);

        public async Task UpdateAsync(Support support) =>
            await _supports.ReplaceOneAsync(s => s.id == support.id, support);
    }
}
