using MongoDB.Driver;
using PaymentAPI.Model;
using PaymentAPI.Repository.Interface;
using Shared.Enums;

namespace PaymentAPI.Repository;

public class PaymentRepository : IPaymentRepository
{
    private readonly IMongoCollection<Payment> _collection;
    
    public PaymentRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<Payment>("Payments");
    }

    

  
    public IQueryable<Payment> GetByOwner(Guid ownerId)
    {
       return _collection.AsQueryable().Where(p => p.OwnerId == ownerId).OrderByDescending(o => o.CreatedDate);
    }

    public IQueryable<Payment> GetByUserId(Guid userId)
    {
        return _collection.AsQueryable().Where(p => p.TenantId == userId).OrderByDescending(o => o.CreatedDate);
    }

    public async Task<Payment?> GetById(Guid id)
    {
        return await _collection.Find(p => p.Id == id).FirstOrDefaultAsync();

    }

    public async Task Add(Payment payment)
    {
        await _collection.InsertOneAsync(payment);
    }

    public async Task Update(Payment payment)
    {
        await _collection.ReplaceOneAsync(p => p.Id == payment.Id, payment);
    }

    public async Task Delete(Guid id)
    {
        await _collection.DeleteOneAsync(p => p.Id == id);
    }

    public  IQueryable<Payment> GetByBillId(Guid billId)
    {
        return  _collection.AsQueryable().Where(p => p.UtilityBillId == billId)
            .OrderByDescending(p => p.CreatedDate);
    }

    public async Task<Payment?> GetByTransactionId(string transactionId)
    {
        return await _collection
            .Find(p => p.TransactionId == transactionId)
            .FirstOrDefaultAsync();
    }

    public async Task<List<Payment>> GetPendingOfflinePaymentsByOwner(Guid ownerId)
    {
        return await _collection
            .Find(p => p.OwnerId == ownerId
                       && p.PaymentMethod == PaymentMethod.Offline
                       && p.Status == PaymentStatus.Pending)
            .SortByDescending(p => p.CreatedDate)
            .ToListAsync();
    }
}
