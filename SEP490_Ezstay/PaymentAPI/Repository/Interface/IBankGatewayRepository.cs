using PaymentAPI.Model;

namespace PaymentAPI.Repository.Interface;

public interface IBankGatewayRepository
{
    Task<BankGateway> GetById(Guid id);
    Task AddMany(IEnumerable<BankGateway> bankGateway);
    Task Update(BankGateway bankGateway);
  //  Task<List<BankGateway>> GetAll();
    Task ClearAll();
    IQueryable<BankGateway> GetAll();
  
}