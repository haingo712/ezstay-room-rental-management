using AmenityAPI.DTO.Request;
using Shared.DTOs.Amenities.Responses;
using Shared.DTOs;

namespace AmenityAPI.Service.Interface;



public interface IAmenityService
{
  //  Task<ApiResponse<IEnumerable<AmenityResponseDto>>>  GetAllByStaffId(Guid staffId);
  //  IQueryable<AmenityResponseDto> GetAllByStaffIdAsQueryable(Guid staffId);
    IQueryable<AmenityResponse> GetAllAsQueryable();
    Task<ApiResponse<IEnumerable<AmenityResponse>>> GetAll();
    Task<AmenityResponse> GetById(Guid id);
   // Task<ApiResponse<AmenityResponseDto>> AddAsync(Guid staffId, CreateAmenityDto request);
    Task<ApiResponse<AmenityResponse>> Add(CreateAmenityDto request);
    Task<ApiResponse<bool>> Update(Guid id,UpdateAmenityDto request);
    Task<ApiResponse<bool>> Delete(Guid id);
    
}