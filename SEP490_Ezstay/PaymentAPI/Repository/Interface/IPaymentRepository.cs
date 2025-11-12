using PaymentAPI.Model;

namespace PaymentAPI.Repository.Interface;

public interface IPaymentRepository
{
    IQueryable<Payment> GetByOwner(Guid ownerId);
    IQueryable<Payment> GetByUserId(Guid userId);
    Task<Payment?> GetById(Guid id);
    Task Add(Payment payment);
    Task Update(Payment payment);
    Task Delete(Guid id);
    IQueryable<Payment> GetByBillId(Guid billId);
    Task<Payment?> GetByTransactionId(string transactionId);
    Task<List<Payment>> GetPendingOfflinePaymentsByOwner(Guid ownerId);
  
}
