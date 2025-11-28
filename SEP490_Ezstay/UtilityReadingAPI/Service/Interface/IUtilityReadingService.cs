
using Shared.DTOs;
using Shared.DTOs.UtilityReadings.Responses;
using Shared.Enums;
using UtilityReadingAPI.DTO.Request;


namespace UtilityReadingAPI.Service.Interface;

public interface IUtilityReadingService
{
    IQueryable<UtilityReadingResponse> GetAllByContractId(Guid contractId);
    Task<UtilityReadingResponse> GetById(Guid id);
    Task<ApiResponse<UtilityReadingResponse>> Add(Guid contractId, UtilityType type, CreateUtilityReadingContract request); 
    Task<ApiResponse<UtilityReadingResponse>> AddUtilityReadingContract(Guid contractId,UtilityType type, CreateUtilityReadingContract request);
    Task<ApiResponse<bool>> UpdateUtilityReadingContract(Guid contractId, UtilityType type, UpdateUtilityReading request);
    Task<ApiResponse<bool>> Update(Guid id,UpdateUtilityReading request);
    Task DeleteAsync(Guid id);
    
    
    
    IQueryable<UtilityReadingResponse> GetAllByOwnerId(Guid contractId,  UtilityType type);
   
    Task<UtilityReadingResponse> GetLastestReading(Guid contractId, UtilityType type, int month, int year);
  
   
    
}