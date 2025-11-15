using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;
using Shared.DTOs.UtilityReadings.Responses;
using Shared.Enums;
using UtilityReadingAPI.DTO.Request;
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
    public IQueryable<UtilityReadingResponse> GetAllByOwnerId(Guid roomId, UtilityType type)
    {
        var utilityReading =   _utilityReadingRepository.GetAllAsQueryable()
            .Where(x=> x.RoomId == roomId && x.Type == type);
        return utilityReading.ProjectTo<UtilityReadingResponse>(_mapper.ConfigurationProvider);
    }

    
    public async Task<UtilityReadingResponse> GetByIdAsync(Guid id)
    {
        var utilityReading = await _utilityReadingRepository.GetByIdAsync(id);
        if (utilityReading==null)
            throw new KeyNotFoundException(" UtilityReading Id not found");
        return _mapper.Map<UtilityReadingResponse>(utilityReading);
    }
    
    public async Task<UtilityReadingResponse> GetLastestReading(Guid roomId, UtilityType type)
    {
        var utilityReading =await _utilityReadingRepository.GetLatestReading(roomId, type);
        if (utilityReading == null)
            throw new KeyNotFoundException("No utility reading found for the specified room and type.");
        return _mapper.Map<UtilityReadingResponse>(utilityReading);
    }
    public async Task<ApiResponse<UtilityReadingResponse>> AddAsync(Guid roomId, UtilityType type, CreateUtilityReadingContract  request)
    {
        var lastReading = _utilityReadingRepository.GetAllAsQueryable()
            .Where(x => x.RoomId == roomId && x.Type == type)
            .OrderByDescending(x => x.ReadingDate)
            .FirstOrDefault();
       var utilityReading = _mapper.Map<UtilityReading>(request);
        utilityReading.ReadingDate = DateTime.UtcNow;
        utilityReading.Type = type;
        // if (lastReading == null)
        // {
        //     utilityReading.PreviousIndex = 0;
        //     utilityReading.CurrentIndex = request.CurrentIndex;
        // }else {
            if (request.CurrentIndex < lastReading?.CurrentIndex)
            {
                return ApiResponse<UtilityReadingResponse>.Fail("Chỉ số mới không được nhỏ hơn chỉ số tháng trước.");
            }
            utilityReading.PreviousIndex = lastReading.CurrentIndex;
            utilityReading.CurrentIndex = request.CurrentIndex;
        utilityReading.Total = request.Price * utilityReading.Consumption;
        //}
        utilityReading.RoomId = roomId;
        await _utilityReadingRepository.AddAsync(utilityReading);
        var result = _mapper.Map<UtilityReadingResponse>(utilityReading);
        return ApiResponse<UtilityReadingResponse>.Success(result, $"Thêm chỉ số {type} thành công.");
    }
    
    // public async Task<ApiResponse<UtilityReadingResponse>> AddAsync(Guid roomId, CreateUtilityReading request)
    // {
    //     var lastReading = _utilityReadingRepository.GetAllAsQueryable()
    //         .Where(x => x.RoomId == roomId && x.Type == request.Type)
    //         .OrderByDescending(x => x.ReadingDate)
    //         .FirstOrDefault();
    //     
    //     // if (await _utilityReadingRepository.ExistsUtilityReadingInMonthAsync(request.RoomId, request.Type, DateTime.UtcNow)) 
    //     //     return ApiResponse<UtilityReadingResponseDto>.Fail("Đã tồn tại chỉ số cho phòng này trong tháng này.");
    //
    //     var utilityReading = _mapper.Map<UtilityReading>(request);
    //     utilityReading.ReadingDate = DateTime.UtcNow;
    //     if (lastReading == null)
    //     {
    //         utilityReading.PreviousIndex = 0;
    //         utilityReading.CurrentIndex = request.CurrentIndex;
    //     }else {
    //         if (request.CurrentIndex < lastReading.CurrentIndex)
    //         {
    //             return ApiResponse<UtilityReadingResponse>.Fail("Chỉ số mới không được nhỏ hơn chỉ số tháng trước.");
    //         }
    //         // utilityReading.PreviousIndex = lastReading?.CurrentIndex ?? request.CurrentIndex;
    //         utilityReading.PreviousIndex = lastReading.CurrentIndex;
    //         utilityReading.CurrentIndex = request.CurrentIndex;
    //         utilityReading.Total = request.Price * utilityReading.Consumption;
    //     }
    //     utilityReading.RoomId = roomId;
    //     await _utilityReadingRepository.AddAsync(utilityReading);
    //     var result = _mapper.Map<UtilityReadingResponse>(utilityReading);
    //     return ApiResponse<UtilityReadingResponse>.Success(result, $"Thêm chỉ số {request.Type} thành công.");
    // }
    public async Task<ApiResponse<UtilityReadingResponse>> AddUtilityReadingContract(Guid roomId,UtilityType utilityType, CreateUtilityReadingContract  request)
    {
      
        var utilityReading = _mapper.Map<UtilityReading>(request);
        utilityReading.ReadingDate = DateTime.UtcNow;
        utilityReading.RoomId = roomId;
        utilityReading.Type = utilityType;
        await _utilityReadingRepository.AddAsync(utilityReading);
        var result = _mapper.Map<UtilityReadingResponse>(utilityReading);
        return ApiResponse<UtilityReadingResponse>.Success(result, "Thêm chỉ số thành công.");
    }
    public async Task<ApiResponse<bool>> UpdateContract(Guid roomId,UtilityType utilityType, UpdateUtilityReading request)
    {
        var reading = _utilityReadingRepository.GetAllAsQueryable()
        .Where(x => x.RoomId == roomId && x.Type == utilityType)
        .OrderByDescending(x => x.ReadingDate)
        .FirstOrDefault();
        if (reading == null)
            return ApiResponse<bool>.Fail("Không tìm thấy chỉ số điện cho phòng này.");
        _mapper.Map(request, reading);
        reading.UpdatedAt = DateTime.UtcNow;
        await _utilityReadingRepository.UpdateAsync(reading);
        return ApiResponse<bool>.Success(true, "Cập nhật điện thành công");
    }
    

    public async Task<ApiResponse<bool>> UpdateAsync(Guid id, UpdateUtilityReading request)
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
         utilityReading.Total = request.Price * utilityReading.Consumption;
         await _utilityReadingRepository.UpdateAsync(utilityReading);
        var result = _mapper.Map<UtilityReadingResponse>(utilityReading);
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