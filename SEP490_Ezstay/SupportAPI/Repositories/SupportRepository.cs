using MongoDB.Driver;
using SupportAPI.Model;
using SupportAPI.Repositories.Interfaces;


namespace SupportAPI.Repositories
{
    public class SupportRepository : ISuportRepository
    {
        private readonly IMongoCollection<SupportModel> _supports;

        public SupportRepository(IMongoDatabase database)
        {
            _supports = database.GetCollection<SupportModel>("SupportModel");
        }

        public async Task<List<SupportModel>> GetAllAsync() =>
            await _supports.Find(_ => true).ToListAsync();

        public async Task<SupportModel> GetByIdAsync(Guid id) =>
            await _supports.Find(s => s.id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(SupportModel support) =>
            await _supports.InsertOneAsync(support);

        public async Task UpdateAsync(SupportModel support) =>
            await _supports.ReplaceOneAsync(s => s.id == support.id, support);
    }
}
