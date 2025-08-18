using AmenityAPI.DTO.Request;
using AmenityAPI.DTO.Response;

namespace AmenityAPI.Service.Interface;

public interface IAmenityService
{
    Task<IEnumerable<AmenityDto>> GetAllByOwnerId(Guid ownerId);
    IQueryable<AmenityDto> GetAllByOwnerIdOdata(Guid ownerId);
    Task<AmenityDto> GetByIdAsync(Guid id);
    Task<ApiResponse<AmenityDto>> AddAsync(CreateAmenityDto request);
    Task<IEnumerable<String>> GetAllDistinctNameAsync();
    Task<ApiResponse<bool>> UpdateAsync(Guid id,UpdateAmenityDto request);
    Task DeleteAsync(Guid id);
    
}