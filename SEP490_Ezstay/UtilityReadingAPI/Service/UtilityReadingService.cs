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

    public IQueryable<UtilityReadingResponse> GetAllByContractId(Guid contractId)
    {
        var utilityReading = _utilityReadingRepository.GetAllByContractId(contractId);
        return utilityReading.ProjectTo<UtilityReadingResponse>(_mapper.ConfigurationProvider);
    }

    public IQueryable<UtilityReadingResponse> GetAllByOwnerId(Guid contractId, UtilityType type)
    {
        var utilityReading =   _utilityReadingRepository.GetAll()
            .Where(x=> x.ContractId == contractId && x.Type == type);
        return utilityReading.ProjectTo<UtilityReadingResponse>(_mapper.ConfigurationProvider);
    }
    
    public async Task<UtilityReadingResponse> GetById(Guid id)
    {
        var utilityReading = await _utilityReadingRepository.GetByIdAsync(id);
        if (utilityReading==null)
            throw new KeyNotFoundException(" UtilityReading Id not found");
        return _mapper.Map<UtilityReadingResponse>(utilityReading);
    }
    
    public async Task<UtilityReadingResponse> GetLastestReading(Guid contractId, UtilityType type, int month, int year)
    {
        var utilityReading =await _utilityReadingRepository.GetLatestReading(contractId, type, month, year);
        if (utilityReading == null)
            throw new KeyNotFoundException("No utility reading found for the specified month/year.");
        return _mapper.Map<UtilityReadingResponse>(utilityReading);
    }

    public async Task<UtilityReadingResponse> GetFirstReading(Guid contractId, UtilityType type)
    {
        var reading = await _utilityReadingRepository.GetFirstReading(contractId, type);
        return  _mapper.Map<UtilityReadingResponse>(reading);
    }

    public async Task<ApiResponse<UtilityReadingResponse>> Add(Guid contractId, UtilityType type, CreateUtilityReading  request)
    {
        
        var lastReading = _utilityReadingRepository.GetAll()
            .Where(x => x.ContractId == contractId && x.Type == type)
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
                return ApiResponse<UtilityReadingResponse>.Fail("The new reading cannot be lower than the previous month's reading.");
            }
            utilityReading.PreviousIndex = lastReading.CurrentIndex;
            utilityReading.CurrentIndex = request.CurrentIndex;
           utilityReading.Total = request.Price * utilityReading.Consumption;
        //}
        utilityReading.ContractId = contractId;
        await _utilityReadingRepository.AddAsync(utilityReading);
        var result = _mapper.Map<UtilityReadingResponse>(utilityReading);
        return ApiResponse<UtilityReadingResponse>.Success(result, "Create Successfully");
    }
    public async Task<ApiResponse<bool>> Update(Guid id, UpdateUtilityReading request)
    {
        var utilityReading =await _utilityReadingRepository.GetByIdAsync(id);
        if (utilityReading==null)
            throw new KeyNotFoundException("Id not found");
        
        if (request.CurrentIndex < utilityReading.PreviousIndex)
            return ApiResponse<bool>.Fail("The new reading cannot be lower than the previous month's reading.");
        
        if (DateTime.UtcNow - utilityReading.ReadingDate > TimeSpan.FromHours(1))
            return ApiResponse<bool>.Fail("Đơn này đã quá 1 giờ, không thể cập nhật nữa.");
        
        _mapper.Map(request,utilityReading);
        utilityReading.UpdatedAt = DateTime.UtcNow;
        utilityReading.Total = request.Price * utilityReading.Consumption;
        await _utilityReadingRepository.UpdateAsync(utilityReading);
        var result = _mapper.Map<UtilityReadingResponse>(utilityReading);
        return ApiResponse<bool>.Success(true,"Update Successfully");
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
    
    public async Task<ApiResponse<UtilityReadingResponse>> AddUtilityReadingContract(Guid contractId,UtilityType utilityType, CreateUtilityReading  request)
    {
        var utilityReading = _mapper.Map<UtilityReading>(request);
        utilityReading.ReadingDate = DateTime.UtcNow;
        utilityReading.ContractId = contractId;
        utilityReading.Type = utilityType;
        await _utilityReadingRepository.AddAsync(utilityReading);
        var result = _mapper.Map<UtilityReadingResponse>(utilityReading);
        return ApiResponse<UtilityReadingResponse>.Success(result, "Create Successfully");
    }
    public async Task<ApiResponse<bool>> UpdateUtilityReadingContract(Guid contractId,UtilityType utilityType, UpdateUtilityReading request)
    {
        var reading = _utilityReadingRepository.GetAll()
                .FirstOrDefault(x => x.ContractId == contractId && x.Type == utilityType);
        // .Where(x => x.ContractId == contractId && x.Type == utilityType)
        //  .OrderByDescending(x => x.ReadingDate)
        // .FirstOrDefault();
        
        // if (reading == null)
        //     return ApiResponse<bool>.Fail("Không tìm thấy chỉ số điện cho phòng này.");
        _mapper.Map(request, reading);
        reading.UpdatedAt = DateTime.UtcNow;
        await _utilityReadingRepository.UpdateAsync(reading);
        return ApiResponse<bool>.Success(true, "Update Successfully");
    }
    

  
    
}