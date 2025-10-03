using UtilityBillAPI.Models;

namespace UtilityBillAPI.Repository.Interface
{
    public interface IUtilityBillRepository
    {
        IQueryable<UtilityBill> GetAll();      
        //Task<IEnumerable<UtilityBill>> GetOverdueBills();
        Task<UtilityBill?> GetByIdAsync(Guid id);               
        Task CreateAsync(UtilityBill bill);
        Task UpdateAsync(UtilityBill bill);
        Task DeleteAsync(UtilityBill bill);                
        Task MarkAsPaidAsync(Guid billId, string paymentMethod);
        Task CancelAsync(Guid billId, string? cancelNote);
        //Task MarkAsOverdueAsync(Guid billId);        
    }
}
