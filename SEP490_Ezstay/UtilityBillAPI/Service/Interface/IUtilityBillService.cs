using UtilityBillAPI.DTO.Request;
using UtilityBillAPI.DTO.Response;
using UtilityBillAPI.Enum;

namespace UtilityBillAPI.Service.Interface
{
    public interface IUtilityBillService
    {
        IQueryable<UtilityBillDTO> GetAll();
        IQueryable<UtilityBillDTO> GetAllByOwnerId(Guid ownerId); 
        IQueryable<UtilityBillDTO> GetAllByTenantId(Guid tenantId);        
        //Task<IEnumerable<UtilityBillDTO>> GetOverdueBillsAsync(); 
        Task<UtilityBillDTO?> GetByIdAsync(Guid id);                
        Task<ApiResponse<UtilityBillDTO>> GenerateBillForRoomAsync(Guid ownerId, Guid roomId, Guid? tenantId);
        Task<ApiResponse<bool>> UpdateBillAsync(Guid id, UpdateUtilityBillDTO dto);
        Task<ApiResponse<bool>> MarkAsPaidAsync(Guid billId, string paymentMethod);
        Task<ApiResponse<bool>> CancelAsync(Guid billId, string? cancelNote);
        //Task<ApiResponse<bool>> GenerateBillAsync(Guid ownerId, DateTime dueDate);
        //Task<ApiResponse<bool>> MarkAsOverdueAsync(Guid billId);

    }
}
