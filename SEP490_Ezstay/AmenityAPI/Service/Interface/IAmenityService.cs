using AmenityAPI.DTO.Request;
using AmenityAPI.DTO.Response;

namespace AmenityAPI.Service.Interface;

public interface IAmenityService
{
    //IQueryable<AmenityDto> GetAllOdata();
    Task<IEnumerable<AmenityDto>> GetAll();
    IQueryable<AmenityDto> GetAllByOwnerId(Guid ownerId);
    Task<AmenityDto> GetByIdAsync(int id);
    Task<ApiResponse<AmenityDto>> AddAsync(CreateAmenityDto request);
    Task<IEnumerable<String>> GetAllDistinctNameAsync();
    Task<ApiResponse<bool>> UpdateAsync(int id,UpdateAmenityDto request);
    Task DeleteAsync(int id);
    
}