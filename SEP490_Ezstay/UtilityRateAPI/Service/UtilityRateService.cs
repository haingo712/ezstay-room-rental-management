using AutoMapper;
using AutoMapper.QueryableExtensions;
using UtilityRateAPI.DTO.Request;
using UtilityRateAPI.DTO.Response;
using UtilityRateAPI.Enum;
using UtilityRateAPI.Model;
using UtilityRateAPI.Repository.Interface;
using UtilityRateAPI.Service.Interface;
namespace UtilityRateAPI.Service;

public class UtilityRateService : IUtilityRateService
{
    private readonly IMapper _mapper;
    private readonly IUtilityRateRepository _utilityRateRepository;
    
    public UtilityRateService(IMapper mapper, IUtilityRateRepository utilityRateRepository)
    {
        _mapper = mapper;
        _utilityRateRepository = utilityRateRepository;
    }

    public async Task<ApiResponse<IEnumerable<UtilityRateDto>>> GetAllByOwnerId(Guid ownerId)
    {
        var utilityRates = await _utilityRateRepository.GetAllByOwnerId(ownerId);
        var uRDto = _mapper.Map<IEnumerable<UtilityRateDto>>(utilityRates);
        if (uRDto == null || !uRDto.Any())
        {
            return ApiResponse<IEnumerable<UtilityRateDto>>.Fail("Không có muc  nào.");
        }
        return ApiResponse<IEnumerable<UtilityRateDto>>.Success(uRDto);
    }

    public async Task<ApiResponse<IEnumerable<UtilityRateDto>>> GetAll()
    {
        var entities = await _utilityRateRepository.GetAll();
        var dto = _mapper.Map<IEnumerable<UtilityRateDto>>(entities);

        if (dto == null || !dto.Any())
        {
            return ApiResponse<IEnumerable<UtilityRateDto>>.Fail("Không có muc  nào.");
        }
        return ApiResponse<IEnumerable<UtilityRateDto>>.Success(dto);
    }

    public IQueryable<UtilityRateDto> GetAllOdata()
    {
      var u=  _utilityRateRepository
            .GetAllOdata();
       return    u.ProjectTo<UtilityRateDto>(_mapper.ConfigurationProvider);
    }

    public IQueryable<UtilityRateDto> GetAllByOwnerIdOdata(Guid ownerId)
    {
        var u = _utilityRateRepository
            .GetAllOdata()
            .Where(x => x.OwnerId== ownerId);
       return u.ProjectTo<UtilityRateDto>(_mapper.ConfigurationProvider);
    }

    public async Task<UtilityRateDto> GetByIdAsync(Guid id)
    {
        var entity = await _utilityRateRepository.GetByIdAsync(id);
        if (entity == null)
            throw new KeyNotFoundException("UtilityRateId not found");

        return _mapper.Map<UtilityRateDto>(entity);
    }
    
    public async Task<int> GetMaxTierByTypeAndOwnerAsync(Guid ownerId, UtilityType type)
    {
        var rates = await _utilityRateRepository.GetAllByOwnerAndTypeAsync(ownerId, type);
        return rates.Any() ? rates.Max(x => x.Tier) : 0;
    }

    public async Task<ApiResponse<UtilityRateDto>> AddAsync(CreateUtilityRateDto request)
    {
        var maxTier =await GetMaxTierByTypeAndOwnerAsync(request.OwnerId, request.Type);
        var nextTier = maxTier + 1;
        int from;
        if (nextTier == 1)
        {
            from = 0;
            if (request.To <= from)
                return ApiResponse<UtilityRateDto>.Fail($"Giá trị To {request.To} phải lớn hơn {from} ");
        }
        else
        {
            var previousTier = await _utilityRateRepository.GetByOwnerTypeAndTierAsync(request.OwnerId, request.Type, maxTier);
            from = previousTier.To + 1;
            if (request.To <= from)
                return ApiResponse<UtilityRateDto>.Fail($"Giá trị To {request.To} phải lớn hơn {from}");
        }
        var utilityRate = _mapper.Map<UtilityRate>(request);
        utilityRate.Tier = nextTier;
        utilityRate.From = from;
        utilityRate.To = request.To;
        utilityRate.CreatedAt = DateTime.UtcNow;
        await _utilityRateRepository.AddAsync(utilityRate);
        var dto = _mapper.Map<UtilityRateDto>(utilityRate);
        return ApiResponse<UtilityRateDto>.Success(dto, "Thêm thành công mức");
    }
    public async Task<ApiResponse<bool>> UpdateAsync(Guid id, UpdateUtilityRateDto request)
    {
        try
        {
            var entity = await _utilityRateRepository.GetByIdAsync(id);
            if (entity == null)
                return ApiResponse<bool>.Fail("Không tìm thấy mức giá");
            _mapper.Map(request, entity); 
            entity.UpdatedAt = DateTime.UtcNow;
            await _utilityRateRepository.UpdateAsync(entity);
           
            var allType = await _utilityRateRepository.GetAllByOwnerAndTypeAsync(entity.OwnerId,entity.Type);
            var nextTiers = allType
                .Where(r => r.Tier > entity.Tier)
                .OrderBy(r => r.Tier)
                .ToList();
            foreach (var next in nextTiers)
            {
                var newFrom = entity.To + 1;
                if (next.From != newFrom) 
                {
                    next.From = newFrom;
                    next.UpdatedAt = DateTime.UtcNow;
                    await _utilityRateRepository.UpdateAsync(next);
                }
            }

            return ApiResponse<bool>.Success(true, "Cập nhật thành công");
        }
        catch (Exception)
        {
            return ApiResponse<bool>.Fail("Có lỗi xảy ra khi cập nhật");
        }
    }

    
   // public async Task<ApiResponse<bool>> UpdateAsync(Guid id, UpdateUtilityRateDto request)
   // {
   //     try
   //     {
   //         var entity = await _utilityRateRepository.GetByIdAsync(id);
   //         if (entity == null)
   //             return ApiResponse<bool>.Fail("Không tìm thấy mức giá");
   //         var allType = await _utilityRateRepository.GetAllByTypeAsync(request.Type);
   //         var currentTier = allType.FirstOrDefault(r => r.Tier == request.Tier);
   //         currentTier.To = request.To;
   //         currentTier.Price = request.Price;
   //         currentTier.UpdatedAt = DateTime.UtcNow;
   //         await _utilityRateRepository.UpdateAsync(currentTier);
   //         var nextTiers = allType.Where(r => r.Tier > request.Tier)
   //                               .OrderBy(r => r.Tier)
   //                               .ToList();
   //         // foreach (var next in nextTiers)
   //         // {
   //         //     next.From = request.To+ 1;
   //         //     next.To = next.From+ ( next.To - next.From);
   //         //     await _utilityRateRepository.UpdateAsync(next);
   //         // }
   //         foreach (var next in nextTiers)
   //         {
   //             next.From = request.To + 1;
   //             await _utilityRateRepository.UpdateAsync(next);
   //         }
   //         return ApiResponse<bool>.Success(true, "Cập nhật thành công");
   //     }
   //     catch (Exception ex)
   //     {
   //         return ApiResponse<bool>.Fail("Có lỗi xảy ra khi cập nhật");
   //     }
   // }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _utilityRateRepository.GetByIdAsync(id);
        if (entity == null)
            throw new KeyNotFoundException("UtilityRateId not found");

        await _utilityRateRepository.DeleteAsync(entity);
    }
}