using System.Security.Claims;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using AmenityAPI.DTO.Request;
using AmenityAPI.DTO.Response;
using AmenityAPI.Models;
using AmenityAPI.Repository.Interface;
using AmenityAPI.Service.Interface;

namespace AmenityAPI.Service;



public class AmenityService: IAmenityService
{
    private readonly IMapper _mapper;
    private readonly IAmenityRepository _amenityRepository;
    public AmenityService(IMapper mapper, IAmenityRepository amenityRepository)
    {
        _mapper = mapper;
        _amenityRepository = amenityRepository;
    }
      // public async Task<ApiResponse<IEnumerable<AmenityResponseDto>>> GetAllByStaffId(Guid staffId)
      // {
      //     var amenity =  await _amenityRepository.GetAllByStaffId(staffId);
      //     var c=  _mapper.Map<IEnumerable<AmenityResponseDto>>(amenity);
      //     return ApiResponse<IEnumerable<AmenityResponseDto>>.Success(c, "ok");
      // }
      public async Task<ApiResponse<IEnumerable<AmenityResponseDto>>> GetAll()
      {
         var result =  _mapper.Map<IEnumerable<AmenityResponseDto>>(await _amenityRepository.GetAll()) ;
         if (result == null)
         {
             return ApiResponse<IEnumerable<AmenityResponseDto>>.Fail("Không có tiện ích nào.");
         }
           return ApiResponse<IEnumerable<AmenityResponseDto>>.Success(result);
      }
      public IQueryable<AmenityResponseDto> GetAllAsQueryable()
      {
          var amenity = _amenityRepository.GetAllAsQueryable();
      
          return amenity.ProjectTo<AmenityResponseDto>(_mapper.ConfigurationProvider);
      }
    // public IQueryable<AmenityResponseDto> GetAllByStaffIdAsQueryable(Guid staffId)
    // {
    //      
    //     var amenity =   _amenityRepository.GetAllAsQueryable().Where(x=> x.StaffId == staffId);
    //   
    //     return amenity.ProjectTo<AmenityResponseDto>(_mapper.ConfigurationProvider);
    // }
    public async Task<AmenityResponseDto> GetByIdAsync(Guid id)
    {
        var amenity = await _amenityRepository.GetByIdAsync(id);
        if (amenity == null)
            throw new KeyNotFoundException("AmentityId not found");
        return   _mapper.Map<AmenityResponseDto>(amenity);
    }
    public async  Task<ApiResponse<AmenityResponseDto>> AddAsync(CreateAmenityDto request )
    { 
        var exist = await _amenityRepository.AmenityNameExistsAsync(request.AmenityName);
        if (exist)
            return ApiResponse<AmenityResponseDto>.Fail("Tiện ích đã có rồi.");
        var amenity = _mapper.Map<Amenity>(request);
        amenity.CreatedAt = DateTime.UtcNow;
        await _amenityRepository.AddAsync(amenity);
        var result =_mapper.Map<AmenityResponseDto>(amenity);
        return  ApiResponse<AmenityResponseDto>.Success(result,"Thêm tiện ích thành công");
    }

    public async Task<ApiResponse<bool>> UpdateAsync(Guid id, UpdateAmenityDto request)
    {
        var amenity =await _amenityRepository.GetByIdAsync(id);
        if (amenity == null)
            throw new KeyNotFoundException("AmentityId not found");
        var existAmentityName = await _amenityRepository.AmenityNameExistsAsync(request.AmenityName);
        if(existAmentityName)
            return ApiResponse<bool>.Fail("Tiện ích đã có rồi.");
        
         _mapper.Map(request, amenity);
         amenity.UpdatedAt = DateTime.UtcNow;
         await _amenityRepository.UpdateAsync(amenity);
        var result = _mapper.Map<AmenityResponseDto>(amenity);
        return ApiResponse<bool>.Success(true,"Cập nhật tiện ích thành công");
    }
    public async Task DeleteAsync(Guid id)
    {
        var amenity = await _amenityRepository.GetByIdAsync(id);
        if (amenity==null) 
            throw new KeyNotFoundException("AmentityId not found");
        await _amenityRepository.DeleteAsync(amenity);
    }
}