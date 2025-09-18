using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using UtilityReadingAPI.DTO.Request;
using UtilityReadingAPI.DTO.Response;
using UtilityReadingAPI.Enum;
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
    public IQueryable<UtilityReadingResponseDto> GetAllByOwnerId(Guid roomId, UtilityType type)
    {
        var utilityReading =   _utilityReadingRepository.GetAllAsQueryable()
            .Where(x=> x.RoomId == roomId && x.Type == type);
        return utilityReading.ProjectTo<UtilityReadingResponseDto>(_mapper.ConfigurationProvider);
    }

    public async Task<UtilityReadingResponseDto> GetByIdAsync(Guid id)
    {
        var utilityReading = await _utilityReadingRepository.GetByIdAsync(id);
        if (utilityReading==null)
            throw new KeyNotFoundException(" UtilityReading Id not found");
         return   _mapper.Map<UtilityReadingResponseDto>(utilityReading);
    }
   
    
    
    public async Task<ApiResponse<UtilityReadingResponseDto>> AddAsync(Guid roomId, CreateUtilityReadingDto request)
    {
        var lastReading = _utilityReadingRepository.GetAllAsQueryable()
            .Where(x => x.RoomId == roomId && x.Type == request.Type)
            .OrderByDescending(x => x.ReadingDate)
            .FirstOrDefault();
        
        // if (await _utilityReadingRepository.ExistsUtilityReadingInMonthAsync(request.RoomId, request.Type, DateTime.UtcNow)) 
        //     return ApiResponse<UtilityReadingResponseDto>.Fail("Đã tồn tại chỉ số cho phòng này trong tháng này.");

        var utilityReading = _mapper.Map<UtilityReading>(request);
        utilityReading.ReadingDate = DateTime.UtcNow;
        if (lastReading == null)
        {
            utilityReading.PreviousIndex = 0;
            utilityReading.CurrentIndex = request.CurrentIndex;
        }else {
            if (request.CurrentIndex < lastReading.CurrentIndex)
            {
                return ApiResponse<UtilityReadingResponseDto>.Fail("Chỉ số mới không được nhỏ hơn chỉ số tháng trước.");
            }
            // utilityReading.PreviousIndex = lastReading?.CurrentIndex ?? request.CurrentIndex;
            utilityReading.PreviousIndex = lastReading.CurrentIndex;
            utilityReading.CurrentIndex = request.CurrentIndex;
        }

        //Console.W("ss"  + utilityReading.CurrentIndex);
        utilityReading.RoomId = roomId;
        await _utilityReadingRepository.AddAsync(utilityReading);
        var result = _mapper.Map<UtilityReadingResponseDto>(utilityReading);
        return ApiResponse<UtilityReadingResponseDto>.Success(result, $"Thêm chỉ số {request.Type} thành công.");
    }


    public async Task<ApiResponse<bool>> UpdateAsync(Guid id, UpdateUtilityReadingDto request)
    {
        var utilityReading =await _utilityReadingRepository.GetByIdAsync(id);
        if (utilityReading==null)
            throw new KeyNotFoundException("Id not found");
        
        if (request.CurrentIndex < utilityReading.PreviousIndex)
            return ApiResponse<bool>.Fail("Chỉ số mới không được nhỏ hơn chỉ số tháng trước.");
        
        if (DateTime.UtcNow - utilityReading.ReadingDate > TimeSpan.FromHours(1))
            return ApiResponse<bool>.Fail("Đơn này đã quá 1 giờ, không thể cập nhật nữa.");
        
         _mapper.Map(request,utilityReading);
         utilityReading.UpdatedAt = DateTime.UtcNow;
         await _utilityReadingRepository.UpdateAsync(utilityReading);
        var result = _mapper.Map<UtilityReadingResponseDto>(utilityReading);
        return ApiResponse<bool>.Success(true,"Cập nhật thành công");
    }

    public async Task DeleteAsync(Guid id)
    {
        var utilityReading = await _utilityReadingRepository.GetByIdAsync(id);
        if (utilityReading == null)
        {
            throw new KeyNotFoundException("Id not found");
        }
         await _utilityReadingRepository.DeleteAsync(utilityReading);
    }
    
}