using Shared.DTOs; 
using UtilityBillAPI.DTO;

namespace UtilityBillAPI.Service.Interface
{
    public interface IUtilityBillService
    {
        IQueryable<UtilityBillDTO> GetAll();
        IQueryable<UtilityBillDTO> GetAllByOwnerId(Guid ownerId); 
        IQueryable<UtilityBillDTO> GetAllByTenantId(Guid tenantId);                
        Task<UtilityBillDTO?> GetByIdAsync(Guid id);
        Task<ApiResponse<UtilityBillDTO>> GenerateMonthlyBillAsync(Guid contractId, Guid ownerId);
        Task<ApiResponse<UtilityBillDTO>> GenerateDepositBillAsync(Guid contractId, Guid ownerId); 
        Task<ApiResponse<bool>> MarkAsPaidAsync(Guid billId);
        Task<ApiResponse<bool>> CancelAsync(Guid billId, string? reason);
        
        // Financial Statistics for Owner
        Task<OwnerFinancialStatisticsResponse> GetFinancialStatisticsAsync(Guid ownerId, int? year = null);
        
        // System-wide Financial Statistics for Admin/Staff
        Task<SystemFinancialStatisticsResponse> GetSystemFinancialStatisticsAsync(int? year = null);
    }
}
