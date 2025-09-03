// using TenantAPI.DTO.Request;
// using TenantAPI.DTO.Response;
// using TenantAPI.Models;
//
// namespace TenantAPI.Service.Interface;
//
// public interface ITenantService
// {
//     IQueryable<TenantDto> GetAll();
//   
//     IQueryable<TenantDto> GetAllByUserId(Guid userId);
//     IQueryable<TenantDto> GetAllByRoomId(int roomId);
//     Task<TenantDto?> GetByIdAsync(int id);
//     Task<ApiResponse<TenantDto>> AddAsync(CreateTenantDto request);
//     Task<ApiResponse<TenantDto>> UpdateAsync(int id, UpdateTenantDto request);
//    // Task DeleteAsync(int id);
// }