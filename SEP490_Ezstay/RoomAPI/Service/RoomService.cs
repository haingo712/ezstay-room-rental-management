using AutoMapper;
using AutoMapper.QueryableExtensions;
using RoomAPI.APIs.Interfaces;
using RoomAPI.DTO.Request;
using Shared.Enums;
using RoomAPI.Model;
using RoomAPI.Repository.Interface;
using RoomAPI.Service.Interface;
using Shared.DTOs;
using Shared.DTOs.RoomAmenities.Responses;
using Shared.DTOs.Rooms.Responses;

namespace RoomAPI.Service;

public class RoomService(
    IRoomRepository _roomRepository,
    IRoomAmenityClientService _roomAmenityClient,
    IAmenityClientService _amenityClient,
    IImageClientService _imageClient,
    IRentalPostClientService _rentalPostClient,
    IContractClientService _contractClient,
    IMapper _mapper
) : IRoomService{
    public IQueryable<RoomResponse> GetAllStatusActiveByHouseId(Guid houseId)
    {
        var rooms = _roomRepository. GetAllStatusActiveByHouseId(houseId, RoomStatus.Available);
     
        return rooms.ProjectTo<RoomResponse>(_mapper.ConfigurationProvider);
    }
    
    public IQueryable<RoomResponse> GetAllByHouseId(Guid houseId)
    {
      //  var rooms = _roomRepository. GetAll().Where(x => x.HouseId == houseId);
      var rooms = _roomRepository.GetAllByHouseId(houseId);
        return rooms.ProjectTo<RoomResponse>(_mapper.ConfigurationProvider);
    }
    public IQueryable<RoomResponse>  GetAll()
    {
        var book = _roomRepository.GetAll();
        return book.ProjectTo<RoomResponse>(_mapper.ConfigurationProvider);
    }

    public async Task<RoomResponse> GetById(Guid id)
    {
        var room = await _roomRepository.GetById(id);
      return   _mapper.Map<RoomResponse>(room);
    }
    // public async Task<ApiResponse<RoomDto>> Add(  Guid houseId, Guid houseLocationId,  CreateRoomDto request)
    // { 
    //     var exist = await _roomRepository.RoomNameExistsInHouse(houseId, request.RoomName, houseLocationId);
    //     if (exist)
    //         return ApiResponse<RoomDto>.Fail("Tên phòng đã tồn tại trong nhà trọ.");
    //     var room = _mapper.Map<Room>(request);
    //     room.HouseId = houseId;
    //     room.HouseLocationId = houseLocationId;
    //     room.RoomStatus= RoomStatus.Available;
    //     room.CreatedAt = DateTime.UtcNow;
    //     await _roomRepository.Add(room);
    //     var result = _mapper.Map<RoomDto>(room);
    //     return ApiResponse<RoomDto>.Success(result, "Thêm phòng thành công");
    // }
    
    public async Task<ApiResponse<RoomResponse>> Add(Guid houseId,  CreateRoom request)
    { 
        var exist = await _roomRepository.RoomNameExistsInHouse(houseId, request.RoomName);
        if (exist)
            return ApiResponse<RoomResponse>.Fail("Tên phòng đã tồn tại trong nhà trọ.");
        var room = _mapper.Map<Room>(request);
        room.ImageUrl= _imageClient.UploadMultipleImage(request.ImageUrl).Result;
        room.HouseId = houseId;
        room.RoomStatus= RoomStatus.Available;
        room.CreatedAt = DateTime.UtcNow;
        await _roomRepository.Add(room);
        var result = _mapper.Map<RoomResponse>(room);
        return ApiResponse<RoomResponse>.Success(result, "Add Room Successfully");
    }

    public async Task<ApiResponse<bool>>  Update(Guid id, UpdateRoom request)
    {
        var checkRoom =await _roomRepository.GetById(id);
        if (checkRoom == null)
            throw new KeyNotFoundException("Room not found");
        var existRoomName = await _roomRepository.RoomNameExistsInHouse(checkRoom.HouseId, request.RoomName, id);
        if(existRoomName)
            return ApiResponse<bool>.Fail("Room name already exists in house");    
        if (request.RoomStatus == RoomStatus.Occupied)
            return ApiResponse<bool>.Fail("K dc set trang thai nay");  
         _mapper.Map(request, checkRoom);
         checkRoom.UpdatedAt = DateTime.UtcNow;
         checkRoom.ImageUrl= _imageClient.UploadMultipleImage(request.ImageUrl).Result;
         await _roomRepository.Update(checkRoom);
         return  ApiResponse<bool>.Success(true, "Update Room Successfully");
    }
    public async Task<ApiResponse<bool>> Delete(Guid id)
    {
        var room = await _roomRepository.GetById(id);
        if (room==null) 
            throw new KeyNotFoundException("k tim thay phong tro");
        var hasPosts = await _rentalPostClient.RentalPostExistsByRoomId(room.Id);
        if (hasPosts)
            return  ApiResponse<bool>.Fail("Can not delete room because it has rental posts.");
        
        var checkContract = await _contractClient.ContractExistsByRoomId(room.Id);
        if (checkContract)
            return  ApiResponse<bool>.Fail("Can not delete room because it has contracts.");
        
        await _roomAmenityClient.DeleteAmenityByRoomId(room.Id);
        await _roomRepository.Delete(room);
        
        return ApiResponse<bool>.Success(true, "Delete Room Successfully");
    }
    public async Task<RoomWithAmenitiesResponse> GetRoomWithAmenitiesAsync(Guid id)
    {
        var roomId = await _roomRepository.GetById(id);
        var room = _mapper.Map<RoomResponse>(roomId);
        // var roomAmenities = await _roomAmenityClient.GetAmenityIdsByRoomId(id);
        // var amenities = new List<AmenityDto>();
        // foreach (var x in roomAmenities)
        // {
        //     var amenity = await _amenityClient.GetAmenityById(x.AmenityId);
        //     amenities.Add(amenity);
        // }
        var roomDto =   _mapper.Map<RoomWithAmenitiesResponse>(room);
        //return new RoomWithAmenitiesDto
       // {
        //    Room = room,
        //roomDto.Amenities = amenities;
        return roomDto;
          //  Amenities = amenities
       // };
    }
    
    public async Task<ApiResponse<bool>> UpdateStatusAsync(Guid roomId,RoomStatus roomStatus)
    {
        var room = await _roomRepository.GetById(roomId);
        if (room == null)
            throw new KeyNotFoundException("Room not found");
      
        // Chuyển string -> Enum
        // if (!Enum.RoomStatus.TryParse<RoomStatus>(roomStatus, true, out var roomStatuss))
        //     return ApiResponse<bool>.Fail("Invalid room status");

        room.RoomStatus = roomStatus;
        room.UpdatedAt = DateTime.UtcNow;
        await _roomRepository.Update(room);
        return ApiResponse<bool>.Success(true, "Cập nhật trạng thái phòng thành công");
    }
}