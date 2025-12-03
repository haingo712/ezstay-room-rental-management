using PaymentAPI.Model;

namespace PaymentAPI.Repository.Interface;

public interface IPaymentRepository
{
    Task<Payment>GetByIdAsync(Guid id);
   // Task<List<Payment>> GetByPaymentIdAsync(Guid paymentId);
  //  Task<List<Payment>> GetByBillIdAsync(Guid billId);
    Task<Payment> GetBySePayTransactionIdAsync(string transactionId);
    Task<Payment> CreateAsync(Payment payment);
    Task<bool> ExistsByTransactionIdAsync(string transactionId);

  
}
