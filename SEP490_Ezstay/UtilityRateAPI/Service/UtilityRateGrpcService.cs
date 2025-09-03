using Grpc.Core;
using UtilityRateAPI.Enum;
using UtilityRateAPI.Repository.Interface;
namespace UtilityRateAPI.Grpc;

 public class UtilityRateGrpcService:UtilityRateService.UtilityRateServiceBase 
{
    private readonly IUtilityRateRepository _rateRepository;

    public UtilityRateGrpcService(IUtilityRateRepository rateRepository)
    {
        _rateRepository = rateRepository;
    }
    public override async Task<CalculateTotalResponse> CalculateTotalWithDetails(
        CalculateTotalRequest request, ServerCallContext context)
    {
        var type = (UtilityType)request.Type; // enum proto là số
   //     var tiers = await _rateRepository.GetByOwnerTypeAndTierAsync(type);
        decimal consumption = (decimal)request.Consumption;
        decimal total = 0;
        decimal remaining = consumption;

        // foreach (var tier in tiers.OrderBy(t => t.Tier))
        // {
        //     int tierRange = tier.To - tier.From + 1;
        //     decimal apply = Math.Min(remaining, tierRange);
        //     total += apply * tier.Price;
        //     remaining -= apply;
        //     if (remaining <= 0) break;
        // }

        return new CalculateTotalResponse { Total = (double)total };
    }

}