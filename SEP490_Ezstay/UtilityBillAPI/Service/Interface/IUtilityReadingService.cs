using Shared.DTOs.UtilityReadings.Responses;

namespace UtilityBillAPI.Service.Interface
{
    public interface IUtilityReadingService
    {
       Task<UtilityReadingResponse?> GetElectricityReadingAsync(Guid roomId);
       Task<UtilityReadingResponse?> GetWaterReadingAsync(Guid roomId);        
       Task<List<UtilityReadingResponse>?> GetUtilityReadingsAsync(Guid contractId);
    }
}
