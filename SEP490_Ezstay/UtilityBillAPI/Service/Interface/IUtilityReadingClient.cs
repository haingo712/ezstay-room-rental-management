using UtilityBillAPI.DTO.Request;

namespace UtilityBillAPI.Service.Interface
{
    public interface IUtilityReadingClient
    {
       Task<UtilityReadingDTO?> GetElectricityReadingAsync(Guid roomId);
       Task<UtilityReadingDTO?> GetWaterReadingAsync(Guid roomId);
    }
}
