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

    public IQueryable<RoomAmenityResponse> GetAllByRoomId(Guid roonId)
    {
        var book =   _roomAmenityRepository.GetAll().Where(r=> r.RoomId == roonId);
        return book.ProjectTo<RoomAmenityResponse>(_mapper.ConfigurationProvider);
    }
    public IQueryable<RoomAmenityResponse> GetAll()
    {
        var book = _roomAmenityRepository.GetAll();
        return book.ProjectTo<RoomAmenityResponse>(_mapper.ConfigurationProvider);
    }
    public async Task<List<RoomAmenityResponse>> GetRoomAmenitiesByRoomIdAsync(Guid roomId)
    {
        var roomAmenity = await _roomAmenityRepository.GetRoomAmenitiesByRoomIdAsync(roomId);
        if (roomAmenity == null)
            throw new KeyNotFoundException("RoomAmentityId not found");
        return   _mapper.Map<List<RoomAmenityResponse>>(roomAmenity);
    }
    public async Task<RoomAmenityResponse> GetByIdAsync(Guid id)
    {
        var roomAmenity = await _roomAmenityRepository.GetByIdAsync(id);
        if (roomAmenity == null)
            throw new KeyNotFoundException("RoomAmentityId not found");
        return   _mapper.Map<RoomAmenityResponse>(roomAmenity);
    }
    public async Task<bool> CheckAmenity(Guid amenityId)
    {
        return   await _roomAmenityRepository.CheckAmenity(amenityId);
       
    }
    public async  Task<ApiResponse<List<RoomAmenityResponse>>> AddAsync(Guid roomId, List<CreateRoomAmenity> request)
    {
        var existing = await _roomAmenityRepository.GetRoomAmenitiesByRoomIdAsync(roomId);
        var result = new List<RoomAmenityResponse>();
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
            result.Add(_mapper.Map<RoomAmenityResponse>(roomAmenity));
        }
        var requestAmenityIds = request.Select(r => r.AmenityId).ToHashSet();
        var toRemove = existing
            .Where(x => !requestAmenityIds.Contains(x.AmenityId))
            .ToList();
        foreach (var r in toRemove)
        {
            await _roomAmenityRepository.DeleteAsync(r);
        }
        return ApiResponse<List<RoomAmenityResponse>>.Success(result, "Thêm tiện ích vào trọ thành công");
    }
    
}