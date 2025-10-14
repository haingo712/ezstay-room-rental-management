using RoomAPI.DTO.Request;
using Shared.DTOs;
using Shared.DTOs.RoomAmenities.Responses;
using Shared.DTOs.Rooms.Responses;
using Shared.Enums;

namespace RoomAPI.Service.Interface;

public interface IRoomService
{
    IQueryable<RoomResponse>  GetAllQueryable();
    //Task<IEnumerable<RoomDto>> GetAllByHouseId(int houseId);
    IQueryable<RoomResponse> GetAllByHouseId(Guid houseId);
    IQueryable<RoomResponse> GetAllStatusActiveByHouseId(Guid houseId);
    Task<RoomResponse> GetById(Guid id);
   // Task<ApiResponse<RoomDto>> Add(CreateRoomDto request);
    // Task<ApiResponse<RoomDto>> Add(Guid houseId, Guid houseLocationId, CreateRoomDto request);
    Task<ApiResponse<RoomResponse>> Add(Guid houseId, CreateRoom request);
    Task<ApiResponse<bool>> Update(Guid id,UpdateRoom request);
    Task<ApiResponse<bool>> Delete(Guid id);
    Task<RoomWithAmenitiesResponse> GetRoomWithAmenitiesAsync(Guid roomId);
    
    Task<ApiResponse<bool>> UpdateStatusAsync(Guid roomId, RoomStatus roomStatus);

}