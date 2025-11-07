using Shared.DTOs.UtilityBills.Responses;

namespace PaymentAPI.Services.Interfaces;

public interface IUtilityBillService
{
    Task<UtilityBillResponse?> GetBillByIdAsync(Guid billId);
    Task<bool> UpdateBillStatusAsync(Guid billId, string status, DateTime? paymentDate);
  //  Task<List<UtilityBillResponse>> GetBillsByOwnerIdAsync(Guid ownerId);
}
