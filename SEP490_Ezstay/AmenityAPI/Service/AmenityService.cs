using System.Security.Claims;
using AmenityAPI.APIs;
using AmenityAPI.APIs.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using AmenityAPI.DTO.Request;

using AmenityAPI.Models;
using AmenityAPI.Repository.Interface;
using AmenityAPI.Service.Interface;
using Shared.DTOs.Amenities.Responses;
using Shared.DTOs;

namespace AmenityAPI.Service;



public class AmenityService: IAmenityService
{
    private readonly IMapper _mapper;
    private readonly IAmenityRepository _amenityRepository;
    private readonly IImageAPI _imageClient;
    private readonly IRoomAmenityAPI _roomAmenityAPI;
    public AmenityService(IMapper mapper, IAmenityRepository amenityRepository, IImageAPI imageClient, IRoomAmenityAPI roomAmenityAPI)
    {
        _mapper = mapper;
        _amenityRepository = amenityRepository;
        _imageClient = imageClient;
        _roomAmenityAPI = roomAmenityAPI;
    }

    // public async Task<ApiResponse<IEnumerable<AmenityResponseDto>>> GetAllByStaffId(Guid staffId)
      // {
      //     var amenity =  await _amenityRepository.GetAllByStaffId(staffId);
      //     var c=  _mapper.Map<IEnumerable<AmenityResponseDto>>(amenity);
      //     return ApiResponse<IEnumerable<AmenityResponseDto>>.Success(c, "ok");
      // }
      public async Task<ApiResponse<IEnumerable<AmenityResponse>>> GetAll()
      {
         var result =  _mapper.Map<IEnumerable<AmenityResponse>>(await _amenityRepository.GetAll()) ;
         if (result == null)
         {
             return ApiResponse<IEnumerable<AmenityResponse>>.Fail("Không có tiện ích nào.");
         }
           return ApiResponse<IEnumerable<AmenityResponse>>.Success(result);
      }
      public IQueryable<AmenityResponse> GetAllAsQueryable()
      {
          var amenity = _amenityRepository.GetAllAsQueryable();
      
          return amenity.ProjectTo<AmenityResponse>(_mapper.ConfigurationProvider);
      }
    // public IQueryable<AmenityResponseDto> GetAllByStaffIdAsQueryable(Guid staffId)
    // {
    //      
    //     var amenity =   _amenityRepository.GetAllAsQueryable().Where(x=> x.StaffId == staffId);
    //   
    //     return amenity.ProjectTo<AmenityResponseDto>(_mapper.ConfigurationProvider);
    // }
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
            return ApiResponse<AmenityResponse>.Fail("Tiện ích đã có rồi.");
  
        var amenity = _mapper.Map<Amenity>(request);
         var c=   _imageClient.UploadImage(request.ImageUrl);
         amenity.ImageUrl = c.Result;
        amenity.CreatedAt = DateTime.UtcNow;
        await _amenityRepository.Add(amenity);
        var result =_mapper.Map<AmenityResponse>(amenity);
        return  ApiResponse<AmenityResponse>.Success(result,"Thêm tiện ích thành công");
    }

    public async Task<ApiResponse<bool>> Update(Guid id, UpdateAmenity request)
    {
        var amenity =await _amenityRepository.GetById(id);
        if (amenity == null)
            throw new KeyNotFoundException("AmentityId not found");
        var existAmentityName = await _amenityRepository.AmenityNameExists(request.AmenityName, id);
        if(existAmentityName)
            return ApiResponse<bool>.Fail("Tiện ích đã có rồi.");
         _mapper.Map(request, amenity);
         amenity.UpdatedAt = DateTime.UtcNow;
         amenity.ImageUrl =  _imageClient.UploadImage(request.ImageUrl).Result;
         await _amenityRepository.Update(amenity);
        var result = _mapper.Map<AmenityResponse>(amenity);
        return ApiResponse<bool>.Success(true,"Cập nhật tiện ích thành công");
    }
    public async Task<ApiResponse<bool>> Delete(Guid id)
    {
        var amenity = await _amenityRepository.GetById(id);
        if (amenity==null) 
            throw new KeyNotFoundException("AmentityId not found");
        var check = await _roomAmenityAPI.HasRoomAmenityByAmenityId(id);
        if (check)
            return ApiResponse<bool>.Fail("Không thể xóa tiện ích vì đang được sử dụng trong một hoặc nhiều phòng.");
        await _amenityRepository.Delete(amenity);
        return ApiResponse<bool>.Success(true, "Xoá thành công");
    }
}