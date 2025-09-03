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
    private readonly IMapper _mapper;
    private readonly IRoomRepository _roomRepository;

    public RoomService(IMapper mapper, IRoomRepository roomRepository)
    {
        _mapper = mapper;
        _roomRepository = roomRepository;
    }

    public IQueryable<RoomDto> GetAllByHouseLocationId(Guid houseLocationId)
    {
        var rooms = _roomRepository.GetAllOdata().Where(x => x.HouseLocationId == houseLocationId);
     
        return rooms.ProjectTo<RoomDto>(_mapper.ConfigurationProvider);
    }
    public IQueryable<RoomDto> GetAllByHouseId(Guid houseId)
    {
        var rooms = _roomRepository.GetAllOdata().Where(x => x.HouseId == houseId);
     
        return rooms.ProjectTo<RoomDto>(_mapper.ConfigurationProvider);
    }
    public IQueryable<RoomDto> GetAllOdata()
    {
        var book =   _roomRepository.GetAllOdata().Where(r=> r.RoomStatus == RoomStatus.Available);
    return book.ProjectTo<RoomDto>(_mapper.ConfigurationProvider);
    }

    public async Task<RoomDto> GetById(Guid id)
    {
        var room = await _roomRepository.GetById(id);
      return   _mapper.Map<RoomDto>(room);
    }

    // public async Task<ApiResponse<RoomDto>>  Add(CreateRoomDto request)
    // { 
    //     
    //     var exist = await _roomRepository.RoomNameExistsInHouse(request.HouseId, request.RoomName, request.HouseLocationId);
    //     if (exist)
    //         return ApiResponse<RoomDto>.Fail("Tên phòng đã tồn tại trong nhà trọ.");
    //     var room = _mapper.Map<Room>(request);
    //     room.IsAvailable = true;
    //     room.CreatedAt = DateTime.UtcNow;
    //     await _roomRepository.Add(room);
    //     var result = _mapper.Map<RoomDto>(room);
    //     return ApiResponse<RoomDto>.Success(result, "Thêm phòng thành công");
    // }
    public async Task<ApiResponse<RoomDto>> Add(Guid houseId, Guid houseLocationId,  CreateRoomDto request)
    { 
        
        var exist = await _roomRepository.RoomNameExistsInHouse(houseId, request.RoomName, houseLocationId);
        if (exist)
            return ApiResponse<RoomDto>.Fail("Tên phòng đã tồn tại trong nhà trọ.");
        var room = _mapper.Map<Room>(request);
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
}