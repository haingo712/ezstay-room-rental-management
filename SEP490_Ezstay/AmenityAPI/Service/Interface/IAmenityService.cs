using AmenityAPI.DTO.Request;
using AmenityAPI.DTO.Response;

namespace AmenityAPI.Service.Interface;



public interface IAmenityService
{
  //  Task<ApiResponse<IEnumerable<AmenityResponseDto>>>  GetAllByStaffId(Guid staffId);
  //  IQueryable<AmenityResponseDto> GetAllByStaffIdAsQueryable(Guid staffId);
    IQueryable<AmenityResponseDto> GetAllAsQueryable();
    Task<ApiResponse<IEnumerable<AmenityResponseDto>>> GetAll();
    Task<AmenityResponseDto> GetById(Guid id);
   // Task<ApiResponse<AmenityResponseDto>> AddAsync(Guid staffId, CreateAmenityDto request);
    Task<ApiResponse<AmenityResponseDto>> Add(CreateAmenityDto request);
    Task<ApiResponse<bool>> Update(Guid id,UpdateAmenityDto request);
    Task<ApiResponse<bool>> Delete(Guid id);
    
}