using MongoDB.Driver;
using ServiceAPI.Model;
using ServiceAPI.Repositories.Interfaces;

namespace ServiceAPI.Repositories
{
    public class ServiceItemRepository : IServiceItemRepository
    {
        private readonly IMongoCollection<ServiceItem> _context;

        public ServiceItemRepository(IMongoDatabase database)
        {
            _context = database.GetCollection<ServiceItem>("ServiceItem");
        }

        public async Task CreateServiceAsync(ServiceItem service)
        {
            await _context.InsertOneAsync(service);
        }

        public async Task<List<ServiceItem>> GetAllServicesAsync()
        {
            return await _context.Find(_ => true).ToListAsync();
        }

        public async Task<ServiceItem> GetServiceByIdAsync(Guid id)
        {
            return await _context.Find(s => s.Id == id).FirstOrDefaultAsync();
        }

        public async Task UpdateServiceAsync(Guid id, ServiceItem updatedService)
        {
          
            updatedService.Id = id;

            await _context.ReplaceOneAsync(s => s.Id == id, updatedService);
        }


        public async Task DeleteServiceAsync(Guid id)
        {
            await _context.DeleteOneAsync(s => s.Id == id);
        }       
    }
}
