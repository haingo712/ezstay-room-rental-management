using AutoMapper;
using AutoMapper.QueryableExtensions;
using RoomAPI.DTO.Request;
using RoomAPI.DTO.Response;
using RoomAPI.Enum;
using RoomAPI.Model;
using RoomAPI.Repository.Interface;
using RoomAPI.Service.Interface;

namespace RoomAPI.Service;

public class RoomService: IRoomService
{

    private readonly IRoomRepository _roomRepository;
    private readonly IRoomAmenityClientService _roomAmenityClient;
    private readonly IAmenityClientService _amenityClient;
    private readonly IMapper _mapper;

    public RoomService(IRoomRepository roomRepository, IRoomAmenityClientService roomAmenityClient, IAmenityClientService amenityClient, IMapper mapper)
    {
        _roomRepository = roomRepository;
        _roomAmenityClient = roomAmenityClient;
        _amenityClient = amenityClient;
        _mapper = mapper;
    }

    
    public IQueryable<RoomDto> GetAllByHouseId(Guid houseId)
    {
        var rooms = _roomRepository. GetAllQueryable().Where(x => x.HouseId == houseId);
     
        return rooms.ProjectTo<RoomDto>(_mapper.ConfigurationProvider);
    }
    public IQueryable<RoomDto>  GetAllQueryable()
    {
        var book = _roomRepository.GetAllQueryable();
    return book.ProjectTo<RoomDto>(_mapper.ConfigurationProvider);
    }

    public async Task<RoomDto> GetById(Guid id)
    {
        var room = await _roomRepository.GetById(id);
      return   _mapper.Map<RoomDto>(room);
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
    
    public async Task<ApiResponse<RoomDto>> Add(  Guid houseId,  CreateRoomDto request)
    { 
        var exist = await _roomRepository.RoomNameExistsInHouse(houseId, request.RoomName);
        if (exist)
            return ApiResponse<RoomDto>.Fail("Tên phòng đã tồn tại trong nhà trọ.");
        var room = _mapper.Map<Room>(request);
        room.HouseId = houseId;
        room.RoomStatus= RoomStatus.Available;
        room.CreatedAt = DateTime.UtcNow;
        await _roomRepository.Add(room);
        var result = _mapper.Map<RoomDto>(room);
        return ApiResponse<RoomDto>.Success(result, "Thêm phòng thành công");
    }

    public async Task<ApiResponse<bool>>  Update(Guid id, UpdateRoomDto request)
    {
        var checkRoom =await _roomRepository.GetById(id);
        if (checkRoom == null)
            throw new KeyNotFoundException("Room not found");
        // var existRoomName = await _roomRepository.RoomNameExistsInHouse(checkRoom.HouseId, request.RoomName, id);
        // if(existRoomName)
        //     return ApiResponse<bool>.Fail("Tên phòng đã tồn tại trong nhà trọ.");    
        if (request.RoomStatus == RoomStatus.Occupied)
            return ApiResponse<bool>.Fail("K dc set trang thai nay");  
         _mapper.Map(request, checkRoom);
         checkRoom.UpdatedAt = DateTime.UtcNow;
         await _roomRepository.Update(checkRoom);
         // return _mapper.Map<RoomDto>(checkRoom);
         return  ApiResponse<bool>.Success(true, "Cặp nhật phòng thành công");
    }
    public async Task Delete(Guid id)
    {
        var room = await _roomRepository.GetById(id);
        if (room==null) 
            throw new KeyNotFoundException("k tim thay phong tro");
        await _roomRepository.Delete(room);
    }
    
    
    // public async Task<RoomWithAmenitiesDto> GetRoomWithAmenities(Guid roomId)
    // {
    //     // 1. Lấy room từ DB
    //     var room = await _roomRepository.GetById(roomId);
    //     if (room == null) throw new KeyNotFoundException("Room not found");
    //
    //     // 2. Lấy danh sách amenityId
    //     var amenityIds = await _roomAmenityClient.GetAmenityIdsByRoomId(roomId);
    //
    //     // 3. Gọi sang AmenityAPI lấy chi tiết AmenityDto
    //     var amenities = new List<AmenityDto>();
    //     foreach (var amenityId in amenityIds)
    //     {
    //         var amenity = await _amenityClient.GetAmenityById(amenityId);
    //         if (amenity != null) amenities.Add(amenity);
    //     }
    //
    //     // 4. Map sang RoomWithAmenitiesDto
    //     var roomDto = _mapper.Map<RoomWithAmenitiesDto>(room);
    //     roomDto.Amenities = amenities;
    //
    //     return roomDto;
    // }
    
    public async Task<RoomWithAmenitiesDto> GetRoomWithAmenitiesAsync(Guid id)
    {
        var roomId = await _roomRepository.GetById(id);
        var room = _mapper.Map<RoomDto>(roomId);
        var roomAmenities = await _roomAmenityClient.GetAmenityIdsByRoomId(id);
        var amenities = new List<AmenityDto>();
        foreach (var x in roomAmenities)
        {
            var amenity = await _amenityClient.GetAmenityById(x.AmenityId);
            amenities.Add(amenity);
        }

        return new RoomWithAmenitiesDto
        {
            Room = room,
            Amenities = amenities
        };
    }

}