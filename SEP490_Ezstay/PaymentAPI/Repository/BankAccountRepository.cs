
using MongoDB.Driver;
using PaymentAPI.Model;
using PaymentAPI.Repository.Interface;

namespace PaymentAPI.Repository;

public class BankAccountRepository:IBankAccountRepository
{
    private readonly IMongoCollection<BankAccount> _bankAccounts;
    
    public BankAccountRepository(IMongoDatabase database)
    {
        _bankAccounts= database.GetCollection<BankAccount>("BankAccounts");
    }

    public IQueryable<BankAccount> GetAll(Guid userId)=> _bankAccounts.AsQueryable().Where(x => x.UserId == userId);
    
    public IQueryable<BankAccount> GetDefaultByUserId(Guid userId)
    {
        return  _bankAccounts.AsQueryable().Where(a => a.UserId == userId && a.IsActive);
    }

    public async Task<bool> CheckExistsBankAccount(Guid userId, Guid bankGatewayId, string accountNumber)
    {
       return  await _bankAccounts.Find(a =>
               a.UserId == userId &&
               a.BankGatewayId == bankGatewayId &&
               a.AccountNumber == accountNumber)
           .AnyAsync();
    }

    public async Task<BankAccount?> GetById(Guid id)
    {
      return await _bankAccounts.Find(a => a.Id == id).FirstOrDefaultAsync();
    }
   

    // public async Task<BankAccount?> GetDefaultByUserId(Guid userId)
    // {
    //     return await _collection.Find(a => a.UserId == userId).FirstOrDefaultAsync();
    // }
    
    // public async Task<BankAccount?> GetDefaultByUserIdAsync(Guid userId)
    // {
    //     // Lấy tài khoản default và active
    //     return await _collection.Find(a => a.UserId == userId && a.IsActive && a.IsActive)
    //         .FirstOrDefaultAsync();
    // }
    //
    // public async Task<List<BankAccount>> GetAllByUserIdAsync(Guid userId)
    // {
    //     return await _collection.Find(a => a.UserId == userId && a.IsActive)
    //         .ToListAsync();
    // }

    public async Task Add(BankAccount bankAccount)
    {
        await _bankAccounts.InsertOneAsync(bankAccount);
    }
    
    public async Task Update(BankAccount bankAccount)
    {
        await _bankAccounts.ReplaceOneAsync(a => a.Id == bankAccount.Id, bankAccount);
    }
    public async Task Delete(BankAccount bankAccount)=> await   _bankAccounts.DeleteOneAsync(a => a.Id == bankAccount.Id);
    
    

   
}