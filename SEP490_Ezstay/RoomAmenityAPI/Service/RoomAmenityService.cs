using RoomAmenityAPI.Service.Interface;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using RoomAmenityAPI.DTO.Request;
using RoomAmenityAPI.DTO.Response;
using RoomAmenityAPI.Model;
using RoomAmenityAPI.Repository.Interface;

namespace RoomAmenityAPI.Service;

public class RoomAmenityService: IRoomAmenityService
{
    private readonly IMapper _mapper;
    private readonly IRoomAmenityRepository _roomAmenityRepository;

    public RoomAmenityService(IMapper mapper, IRoomAmenityRepository roomAmenityRepository)
    {
        _mapper = mapper;
        _roomAmenityRepository = roomAmenityRepository;
    }

    public IQueryable<RoomAmenityDto> GetAllByRoomId(Guid roonId)
    {
        var book =   _roomAmenityRepository.GetAll().Where(r=> r.RoomId == roonId);
        return book.ProjectTo<RoomAmenityDto>(_mapper.ConfigurationProvider);
    }
    public IQueryable<RoomAmenityDto> GetAll()
    {
        var book = _roomAmenityRepository.GetAll();
        return book.ProjectTo<RoomAmenityDto>(_mapper.ConfigurationProvider);
    }

    public async Task<List<RoomAmenityDto>> GetRoomAmenitiesByRoomIdAsync(Guid roomId)
    {
        var roomAmenity = await _roomAmenityRepository.GetRoomAmenitiesByRoomIdAsync(roomId);
        if (roomAmenity == null)
            throw new KeyNotFoundException("RoomAmentityId not found");
        return   _mapper.Map<List<RoomAmenityDto>>(roomAmenity);
    }

    public async Task<RoomAmenityDto> GetByIdAsync(Guid id)
    {
        var roomAmenity = await _roomAmenityRepository.GetByIdAsync(id);
        if (roomAmenity == null)
            throw new KeyNotFoundException("RoomAmentityId not found");
        return   _mapper.Map<RoomAmenityDto>(roomAmenity);
    }
    
    // public async  Task<ApiResponse<List<RoomAmenityDto>>> AddAsync(Guid roomId, List<CreateRoomAmenityDto> request)
    // {
    //     var existing = await _roomAmenityRepository.GetRoomAmenitiesByRoomIdAsync(roomId);
    //   
    //     var result = new List<RoomAmenityDto>();
    //     var toAdd = request
    //         .Where(r => !existing.Any(e => e.AmenityId == r.AmenityId))
    //         .ToList();
    //
    //     
    //     foreach (var r in request)
    //     {
    //         var exist = await _roomAmenityRepository.AmenityIdExistsInRoomAsync(roomId, r.AmenityId);
    //         if (exist)
    //             continue; // bỏ qua nếu đã tồn tại trong room
    //         var roomAmenity = _mapper.Map<RoomAmenity>(r);
    //         roomAmenity.RoomId = roomId;
    //         roomAmenity.CreatedAt = DateTime.UtcNow;
    //         await _roomAmenityRepository.AddAsync(roomAmenity);
    //         result.Add(_mapper.Map<RoomAmenityDto>(roomAmenity));
    //     }
      //  var exist = await _roomAmenityRepository.AmenityIdExistsInRoomAsync(roomId, request.AmenityId);
        // if (exist)
        //     return  ApiResponse<RoomAmenityDto>.Fail("Tiện ích đã có tại trong nhà trọ. vui long them tien ich khac");
       // var roomAmenity = _mapper.Map<RoomAmenity>(request);
     //   roomAmenity.RoomId = roomId;
      //  roomAmenity.CreatedAt = DateTime.Now;
      //  await _roomAmenityRepository.AddAsync(roomAmenity);
    //    return ApiResponse<List<RoomAmenityDto>>.Success(result, "Thêm tiện ích vào trọ thành công");
  //  }
    public async  Task<ApiResponse<List<RoomAmenityDto>>> AddAsync(Guid roomId, List<CreateRoomAmenityDto> request)
    {
        var existing = await _roomAmenityRepository.GetRoomAmenitiesByRoomIdAsync(roomId);
        var result = new List<RoomAmenityDto>();
        var toAdd = request
            .Where(r => !existing.Any(e => e.AmenityId == r.AmenityId))
            .ToList();
        foreach (var r in toAdd)
        {
            var exist = await _roomAmenityRepository.AmenityIdExistsInRoomAsync(roomId, r.AmenityId);
            if (exist)
                continue; 
            var roomAmenity = _mapper.Map<RoomAmenity>(r);
            roomAmenity.RoomId = roomId;
            roomAmenity.CreatedAt = DateTime.UtcNow;
            await _roomAmenityRepository.AddAsync(roomAmenity);
            result.Add(_mapper.Map<RoomAmenityDto>(roomAmenity));
        }
        var requestAmenityIds = request.Select(r => r.AmenityId).ToHashSet();
        var toRemove = existing
            .Where(x => !requestAmenityIds.Contains(x.AmenityId))
            .ToList();
        foreach (var r in toRemove)
        {
            await _roomAmenityRepository.DeleteAsync(r);
        }
        return ApiResponse<List<RoomAmenityDto>>.Success(result, "Thêm tiện ích vào trọ thành công");
    }

    public async  Task<ApiResponse<bool>> UpdateAsync(Guid id, UpdateRoomAmenityDto request)
    {
        var roomAmenity =await _roomAmenityRepository.GetByIdAsync(id);
        if (roomAmenity == null)
            throw new KeyNotFoundException("AmentityId not found");
      //  var exist = await _roomAmenityRepository.AmenityIdExistsInRoomAsync(request.RoomId, request.AmenityId);
      //  if (exist)
      //  {
       //    return ApiResponse<bool>.Fail("Tiện ích đã có tại trong nhà trọ. Vui lòng thêm tiện ích khác");
      //  }
         _mapper.Map(request, roomAmenity);
         roomAmenity.UpdatedAt = DateTime.Now;
         await _roomAmenityRepository.UpdateAsync(roomAmenity);
      //  var result =_mapper.Map<RoomAmenityDto>(roomAmenity);
           return ApiResponse<bool>.Success(true,"Cập nhật thành công");
    }
    public async Task DeleteAsync(Guid id)
    {
        var amenity = await _roomAmenityRepository.GetByIdAsync(id);
        if (amenity==null) 
            throw new KeyNotFoundException("k tim thay phong amenity id: " + id);
        await _roomAmenityRepository.DeleteAsync(amenity);
    }
}