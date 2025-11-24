using PaymentAPI.Model;

namespace PaymentAPI.Repository.Interface;

public interface IBankAccountRepository
{   
    IQueryable<BankAccount> GetAll(Guid userId);
    Task<BankAccount?> GetById(Guid id);
    IQueryable<BankAccount> GetDefaultByUserId(Guid userId);
    Task<bool> CheckExistsBankAccount(Guid userId, Guid bankGatewayId, string accountNumber);

    // Task<List<BankAccount>> GetAllByUserIdAsync(Guid userId); // Lấy tất cả tài khoản của owner
    Task Add(BankAccount bankAccount);
    Task Update(BankAccount bankAccount);
    Task Delete(BankAccount bankAccount);
    
    
}