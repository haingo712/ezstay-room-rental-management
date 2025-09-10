namespace PaymentAPI.Services.Interfaces;

public interface IPaymentService
{
  //  Task<string> CreatePaymentUrl(Guid id, decimal amount);
  //  Task<bool> VerifyPayment(IDictionary<string, string> parameters);

  // Task<string> CreatePaymentUrl(Guid id, decimal amount);
  // Task<bool> VerifyPayment(IDictionary<string, string> parameters);
  Task<string> CreatePaymentUrl(Guid id, decimal amount);
  public Task<bool> VerifyPayment(IDictionary<string, string> parameters);
}