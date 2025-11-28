using AutoMapper;
using AutoMapper.QueryableExtensions;
using RoomAPI.DTO.Request;
using RoomAPI.Model;
using RoomAPI.Repository.Interface;
using RoomAPI.Service.Interface;
using Shared.DTOs;
using Shared.DTOs.RoomAmenities.Responses;

namespace RoomAPI.Service;

public class RoomAmenityService: IRoomAmenityService
{
    private readonly IMapper _mapper;
    private readonly IRoomAmenityRepository _roomAmenityRepository;

    public RoomAmenityService(IMapper mapper, IRoomAmenityRepository roomAmenityRepository)
    {
        _mapper = mapper;
        _roomAmenityRepository = roomAmenityRepository;
    }
    
    public IQueryable<RoomAmenityResponse> GetAll()
    {
        var book = _roomAmenityRepository.GetAll();
        return book.ProjectTo<RoomAmenityResponse>(_mapper.ConfigurationProvider);
    }
    
    public IQueryable<RoomAmenityResponse> GetAllByRoomId(Guid roonId)
    {
        var book =   _roomAmenityRepository.GetAllByRoomId(roonId);
        return book.ProjectTo<RoomAmenityResponse>(_mapper.ConfigurationProvider);
    }
    
    public async Task<RoomAmenityResponse> GetById(Guid id)
    {
        var roomAmenity = await _roomAmenityRepository.GetById(id);
        if (roomAmenity == null)
            throw new KeyNotFoundException("Room Amentity not found");
        return   _mapper.Map<RoomAmenityResponse>(roomAmenity);
    }
    public async Task<bool> CheckAmenity(Guid amenityId)
    {
        return  await _roomAmenityRepository.CheckAmenity(amenityId);
    }
    // public async Task<bool> DeleteAmenityByRoomId(Guid roomId)
    // {
    //     var amenities = await _roomAmenityRepository.GetRoomAmenitiesByRoomId(roomId);
    //     foreach (var item in amenities)
    //     {
    //         await _roomAmenityRepository.Delete(item);
    //     }
    //     return true;
    // }
    public async  Task<ApiResponse<List<RoomAmenityResponse>>> UpdateRoomAmenities(Guid roomId, List<Guid>? amenityIds)
    {
        if (amenityIds == null)
            amenityIds = new List<Guid>();
        Console.WriteLine(" ddđ1111 "+ amenityIds);
        var existing = await _roomAmenityRepository.GetListByRoomIdAsync(roomId);
        var existingAmenityIds = existing.Select(e => e.AmenityId).ToList();
        var toAdd = amenityIds
            .Where(id => !existingAmenityIds.Contains(id))
            .Select(id=> new RoomAmenity() {
                Id = Guid.NewGuid(),
                RoomId = roomId,
                AmenityId = id
            }).ToList();
        
        if (toAdd.Any())
        {
            await _roomAmenityRepository.AddAmenity(toAdd);
        } 
        
        var toRemoveIds = existing
            .Where(e => !amenityIds.Contains(e.AmenityId))
            .Select(e => e.Id)
            .ToList();

            
        if (toRemoveIds.Any())
            await _roomAmenityRepository.DeleteAmenity(toRemoveIds);
        
        // Return updated list
        var updatedList = existing
            .Where(x => !toRemoveIds.Contains(x.Id))
            .Concat(toAdd)
            .ToList();
        return ApiResponse<List<RoomAmenityResponse>>.Success(_mapper.Map<List<RoomAmenityResponse>>(updatedList), "Success");
    }
    
}