
using MongoDB.Driver;
using PaymentAPI.Model;
using PaymentAPI.Repository.Interface;

namespace PaymentAPI.Repository;

public class BankAccountRepository:IBankAccountRepository
{
    private readonly IMongoCollection<BankAccount> _collection;
    
    public BankAccountRepository(IMongoDatabase database)
    {
        _collection= database.GetCollection<BankAccount>("BankAccounts");
    }

    public IQueryable<BankAccount> GetAllAsQueryable(Guid userId)=> _collection.AsQueryable().Where(x => x.UserId == userId);
    
    public IQueryable<BankAccount> GetDefaultByUserId(Guid userId)
    {
        return  _collection.AsQueryable().Where(a => a.UserId == userId && a.IsActive);
    }
    
    public async Task<BankAccount?> GetById(Guid id)
    {
      return await _collection.Find(a => a.Id == id).FirstOrDefaultAsync();
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
        await _collection.InsertOneAsync(bankAccount);
    }
    
    public async Task Update(BankAccount bankAccount)
    {
        await _collection.ReplaceOneAsync(a => a.Id == bankAccount.Id, bankAccount);
    }
    public async Task Delete(BankAccount bankAccount)=> await   _collection.DeleteOneAsync(a => a.Id == bankAccount.Id);
    
    

   
}