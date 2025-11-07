using MongoDB.Driver;
using PaymentAPI.Model;
using PaymentAPI.Repository.Interface;

namespace PaymentAPI.Repository;

public class BankGatewayRepository : IBankGatewayRepository
    {
    private readonly IMongoCollection<BankGateway> _collection;

    public BankGatewayRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<BankGateway>("BankGateways");
    }
    public async Task<BankGateway> GetById(Guid id)
    {
        return await _collection.Find(a => a.Id == id).FirstOrDefaultAsync();
    }

    public async Task AddMany(IEnumerable<BankGateway> gateways)
    {
        await _collection.InsertManyAsync(gateways);
    }
    public async Task Update(BankGateway bankGateway)
    {
        await _collection.ReplaceOneAsync(a => a.Id == bankGateway.Id, bankGateway);
    }

    // public async Task<List<BankGateway>> GetAll()
    // {
    //     return await _collection.Find(_ => true).ToListAsync();
    // }
    public IQueryable<BankGateway> GetAll()
    {
        return _collection.AsQueryable();
    }
    
    public async Task ClearAll()
    {
        await _collection.DeleteManyAsync(_ => true);
    }

}