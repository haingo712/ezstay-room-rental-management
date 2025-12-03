using MongoDB.Driver;
using PaymentAPI.Model;
using PaymentAPI.Repository.Interface;

namespace PaymentAPI.Repository;

public class PaymentRepository : IPaymentRepository
{
    private readonly IMongoCollection<Payment> _payment;

    public PaymentRepository(IMongoDatabase database)
    {
        _payment = database.GetCollection<Payment>("Payments");
    }

    public async Task<Payment> GetByIdAsync(Guid id)
    {
        return await _payment.Find(h => h.Id == id).FirstOrDefaultAsync();
    }

    // public async Task<List<Payment>> GetByPaymentIdAsync(Guid paymentId)
    // {
    //     return await _payment.Find(h => h.PaymentId == paymentId)
    //         .SortByDescending(h => h.CreatedAt)
    //         .ToListAsync();
    // }

    // public async Task<List<PaymentHistory>> GetByBillIdAsync(Guid billId)
    // {
    //     return await _histories.Find(h => h.UtilityBillId == billId)
    //         .SortByDescending(h => h.CreatedAt)
    //         .ToListAsync();
    // }

    public async Task<Payment> GetBySePayTransactionIdAsync(string transactionId)
    {
        return await _payment.Find(h => h.TransactionId == transactionId)
            .FirstOrDefaultAsync();
    }

    public async Task<Payment> CreateAsync(Payment history)
    {
        await _payment.InsertOneAsync(history);
        return history;
    }

    public async Task<bool> ExistsByTransactionIdAsync(string transactionId)
    {
        return await _payment.Find(h => h.TransactionId == transactionId).AnyAsync();
    }
}
