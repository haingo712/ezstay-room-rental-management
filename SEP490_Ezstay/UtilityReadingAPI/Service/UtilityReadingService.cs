using AutoMapper;
using AutoMapper.QueryableExtensions;
using UtilityReadingAPI.DTO.Request;
using UtilityReadingAPI.DTO.Response;
using UtilityReadingAPI.Model;
using UtilityReadingAPI.Repository.Interface;
using UtilityReadingAPI.Service.Interface;

namespace UtilityReadingAPI.Service;

public class UtilityReadingService: IUtilityReadingService
{
    private readonly IMapper _mapper;
    private readonly IUtilityReadingRepository _utilityReadingRepository;

    public UtilityReadingService(IMapper mapper, IUtilityReadingRepository utilityReadingRepository)
    {
        _mapper = mapper;
        _utilityReadingRepository= utilityReadingRepository;
    }
    // public IQueryable<UtilityReadingDto> GetAllByOwnerIdOdata(Guid ownerId)
    // {
    //     var utilityReading =   _utilityReadingRepository.GetAllOdata().Where(x=> x.RoomId == ownerId);
    //     return utilityReading.ProjectTo<UtilityReadingDto>(_mapper.ConfigurationProvider);
    // }

    public async Task<UtilityReadingDto> GetByIdAsync(Guid id)
    {
        var utilityReading = await _utilityReadingRepository.GetByIdAsync(id);
        if (utilityReading==null)
            throw new KeyNotFoundException(" UtilityReading Id not found");
         return   _mapper.Map<UtilityReadingDto>(utilityReading);
    }

    public async  Task<ApiResponse<UtilityReadingDto>> AddAsync(CreateUtilityReadingDto request)
    { 
         if (await _utilityReadingRepository.ExistsUtilityReadingInMonthAsync(request.RoomId, request.Type, DateTime.UtcNow)) 
             return  ApiResponse<UtilityReadingDto>.Fail("Đã tồn tại chỉ số cho phòng này trong tháng này.");
        var utilityReading = _mapper.Map<UtilityReading>(request);
       utilityReading.ReadingDate = DateTime.UtcNow;
        await _utilityReadingRepository.AddAsync(utilityReading);
        var result =_mapper.Map<UtilityReadingDto>(utilityReading);
        return  ApiResponse<UtilityReadingDto>.Success(result,"Thêm"+request.Type+" thành công");
    }

    public async Task<ApiResponse<bool>> UpdateAsync(Guid id, UpdateUtilityReadingDto request)
    {
        var utilityReading =await _utilityReadingRepository.GetByIdAsync(id);
        if (utilityReading==null)
            throw new KeyNotFoundException("Id not found");
        if (request.CurrentIndex < utilityReading.PreviousIndex)
        {
            return ApiResponse<bool>.Fail("CurrentIndex k dc nho hon PreviousIndex");
        }
         _mapper.Map(request,utilityReading);
         utilityReading.UpdatedAt = DateTime.UtcNow;
         await _utilityReadingRepository.UpdateAsync(utilityReading);
        var result = _mapper.Map<UtilityReadingDto>(utilityReading);
        return ApiResponse<bool>.Success(true,"Cập nhật thành công");
    }
}