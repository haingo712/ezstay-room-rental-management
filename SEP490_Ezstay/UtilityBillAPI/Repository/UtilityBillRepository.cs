using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using UtilityBillAPI.Enum; 
using UtilityBillAPI.Data;
using UtilityBillAPI.Models;
using UtilityBillAPI.Repository.Interface;

namespace UtilityBillAPI.Repository
{
    public class UtilityBillRepository : IUtilityBillRepository
    {
        private readonly IMongoCollection<UtilityBill> _utilityBills;

        public UtilityBillRepository(MongoDbService service)
        {
            _utilityBills = service.UtilityBills;
        }

        public IQueryable<UtilityBill> GetAll()
        {
            return _utilityBills.AsQueryable().OrderByDescending(b => b.CreatedAt);
        }       

        /*public async Task<IEnumerable<UtilityBill>> GetOverdueBills()
        {
            var now = DateTime.UtcNow;
            return await _utilityBills.Find(b => b.DueDate < now && b.Status == UtilityBillStatus.Unpaid).ToListAsync();
        }*/

        public async Task<UtilityBill?> GetByIdAsync(Guid id)
        {
            return await _utilityBills.Find(b => b.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(UtilityBill bill)
        {          
            await _utilityBills.InsertOneAsync(bill);
        }

        public async Task UpdateAsync(UtilityBill bill)
        {
            bill.UpdatedAt = DateTime.UtcNow;
            await _utilityBills.ReplaceOneAsync(b => b.Id == bill.Id, bill);
        }

        public async Task DeleteAsync(UtilityBill bill)
        {
            await _utilityBills.DeleteOneAsync(b => b.Id == bill.Id);
        }

        public async Task MarkAsPaidAsync(Guid billId, string paymentMethod)
        {
            var update = Builders<UtilityBill>.Update
               .Set(b => b.Status, UtilityBillStatus.Paid)
               .Set(b => b.PaymentMethod, paymentMethod)
               .Set(b => b.PaymentDate, DateTime.UtcNow)
               .Set(b => b.UpdatedAt, DateTime.UtcNow);

            await _utilityBills.UpdateOneAsync(b => b.Id == billId, update);
        }

        public async Task CancelAsync(Guid billId, string? cancelNote)
        {
            var update = Builders<UtilityBill>.Update
                .Set(b => b.Status, UtilityBillStatus.Cancelled)
                .Set(b => b.UpdatedAt, DateTime.UtcNow);

            if (cancelNote != null)
            {
                update = update.Set(b => b.Note, cancelNote);
            }

            await _utilityBills.UpdateOneAsync(b => b.Id == billId, update);
        }

        /*public async Task MarkAsOverdueAsync(Guid billId)
        {
            var update = Builders<UtilityBill>.Update
                .Set(b => b.Status, UtilityBillStatus.Overdue)
                .Set(b => b.UpdatedAt, DateTime.UtcNow);
            await _utilityBills.UpdateOneAsync(b => b.Id == billId, update);

        }*/
    }
}
