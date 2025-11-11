using AmenityAPI.DTO.Request;
using Shared.DTOs.Amenities.Responses;
using Shared.DTOs;

namespace AmenityAPI.Service.Interface;



public interface IAmenityService
{
    IQueryable<AmenityResponse> GetAll();
    Task<AmenityResponse> GetById(Guid id);
    Task<ApiResponse<AmenityResponse>> Add(CreateAmenity request);
    Task<ApiResponse<bool>> Update(Guid id,UpdateAmenity request);
    Task<ApiResponse<bool>> Delete(Guid id);
    
}