using MongoDB.Driver;
using ServiceAPI.Model;
using ServiceAPI.Repositories.Interfaces;

namespace ServiceAPI.Repositories
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly IMongoCollection<ServiceModel> _context;

        public ServiceRepository(IMongoDatabase database)
        {
            _context = database.GetCollection<ServiceModel>("ServiceModel");
        }

        public async Task CreateServiceAsync(Model.ServiceModel service)
        {
            await _context.InsertOneAsync(service);
        }

        public async Task<List<Model.ServiceModel>> GetAllServicesAsync()
        {
            return await _context.Find(_ => true).ToListAsync();
        }

        public async Task<Model.ServiceModel> GetServiceByIdAsync(string id)
        {
            return await _context.Find(s => s.Id == id).FirstOrDefaultAsync();
        }

        public async Task UpdateServiceAsync(string id, Model.ServiceModel updatedService)
        {
          
            updatedService.Id = id;

            await _context.ReplaceOneAsync(s => s.Id == id, updatedService);
        }


        public async Task DeleteServiceAsync(string id)
        {
            await _context.DeleteOneAsync(s => s.Id == id);
        }       
    }
}
