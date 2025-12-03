//
// using RoomAPI.DTO.Request;
// using Shared.DTOs;
// using Shared.DTOs.RoomAmenities.Responses;
//
// namespace RoomAPI.Service.Interface;
//
// public interface IRoomAmenityService
// {
//     IQueryable<RoomAmenityResponse> GetAllByRoomId(Guid roonId);
//     Task<ApiResponse<List<RoomAmenityResponse>>> UpdateRoomAmenities(Guid roomId, List<Guid>? amenityIds);
//     Task<bool> CheckAmenity(Guid amenityId);
//     //Task<bool> DeleteAmenityByRoomId(Guid roomId);
// }