using AutoMapper;
using AutoMapper.QueryableExtensions;
using AmenityAPI.DTO.Request;

using AmenityAPI.Models;
using AmenityAPI.Repository.Interface;
using AmenityAPI.Service.Interface;
using Shared.DTOs.Amenities.Responses;
using Shared.DTOs;

namespace AmenityAPI.Service; 
public class AmenityService : IAmenityService{
    private readonly IMapper _mapper;
    private readonly IAmenityRepository _amenityRepository;
    private readonly IImageService _imageClient;
    private readonly IRoomAmenityService _roomAmenityService;

    public AmenityService(IMapper mapper, IAmenityRepository amenityRepository, IImageService imageClient, IRoomAmenityService roomAmenityService) {
        _mapper = mapper;
        _amenityRepository = amenityRepository;
        _imageClient = imageClient;
        _roomAmenityService = roomAmenityService;
    }
      public IQueryable<AmenityResponse> GetAll()
      {
          var amenity = _amenityRepository.GetAll();
      
          return amenity.ProjectTo<AmenityResponse>(_mapper.ConfigurationProvider);
      }
    public async Task<AmenityResponse> GetById(Guid id)
    {
        var amenity = await _amenityRepository.GetById(id);
        if (amenity == null)
            throw new KeyNotFoundException("AmentityId not found");
        return   _mapper.Map<AmenityResponse>(amenity);
    }
    public async  Task<ApiResponse<AmenityResponse>> Add(CreateAmenity request )
    { 
        var exist = await _amenityRepository.AmenityNameExists(request.AmenityName);
        if (exist)
            return ApiResponse<AmenityResponse>.Fail("Amenity name already exists");
        var amenity = _mapper.Map<Amenity>(request);
        amenity.ImageUrl =   _imageClient.UploadImage(request.ImageUrl).Result;;
        amenity.CreatedAt = DateTime.UtcNow;
        await _amenityRepository.Add(amenity);
        var result =_mapper.Map<AmenityResponse>(amenity);
        return  ApiResponse<AmenityResponse>.Success(result,"Add successfully");
    }
    public async Task<ApiResponse<bool>> Update(Guid id, UpdateAmenity request)
    {
        var amenity =await _amenityRepository.GetById(id);
        if (amenity == null)
            throw new KeyNotFoundException("AmentityId not found");
        var oldAmenityName = amenity.AmenityName; 
      
         var existAmentityName = await _amenityRepository.AmenityNameExists(request.AmenityName);
         
         if (existAmentityName &&  request.AmenityName != oldAmenityName)
             return ApiResponse<bool>.Fail("Amenity name already exists");
        // if (!string.Equals(oldAmenityName, request.AmenityName, StringComparison.OrdinalIgnoreCase))
        // {
        //     if (existAmentityName)
        //         return ApiResponse<bool>.Fail("Amenity name already exists");
        // }
        _mapper.Map(request, amenity);
         amenity.UpdatedAt = DateTime.UtcNow;
         // amenity.ImageUrl = request.ImageUrl;
         await _amenityRepository.Update(amenity);
        // var result = _mapper.Map<AmenityResponse>(amenity);
        return ApiResponse<bool>.Success(true,"Update successfully");
    }
    public async Task<ApiResponse<bool>> Delete(Guid id)
    {
        var amenity = await _amenityRepository.GetById(id);
        if (amenity==null) 
            throw new KeyNotFoundException("AmentityId not found");
        var check = await _roomAmenityService.RoomAmenityExistsByAmenityId(id);
        if (check)
            return ApiResponse<bool>.Fail("Cannot delete this amenity because it is being used");
        await _amenityRepository.Delete(amenity);
        return ApiResponse<bool>.Success(true, "Delete successfully");
    }
}