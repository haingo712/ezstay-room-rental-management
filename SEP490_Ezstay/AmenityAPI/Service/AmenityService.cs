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
    private readonly IHttpContextAccessor _httpContextAccessor;
    public AmenityService(IMapper mapper, IAmenityRepository amenityRepository, IHttpContextAccessor httpContextAccessor)
    {
        _mapper = mapper;
        _amenityRepository = amenityRepository;
        _httpContextAccessor = httpContextAccessor;
     
    }
    // private Guid GetStaffIdFromToken()
    // {
    //     var staffIdStr = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    //     if (string.IsNullOrEmpty(staffIdStr))
    //         throw new UnauthorizedAccessException("Không xác định được StaffId từ token.");
    //     return Guid.Parse(staffIdStr);
    // }
     // public async Task<ApiResponse<IEnumerable<AmenityDto>>> GetAllByStaffId(Guid staffId)
     //  {
     //      var staffIdStr = GetStaffIdFromToken();
     //       var amenity =  await _amenityRepository.GetAllByStaffId(staffIdStr);
     //  var c=  _mapper.Map<IEnumerable<AmenityDto>>(amenity);
     //  return ApiResponse<IEnumerable<AmenityDto>>.Success(c, "ok");
     //  }
      public async Task<ApiResponse<IEnumerable<AmenityDto>>> GetAllByStaffId(Guid staffId)
      {
          var amenity =  await _amenityRepository.GetAllByStaffId(staffId);
          var c=  _mapper.Map<IEnumerable<AmenityDto>>(amenity);
          return ApiResponse<IEnumerable<AmenityDto>>.Success(c, "ok");
      }
      public async Task<ApiResponse<IEnumerable<AmenityDto>>> GetAll()
      {
         var result =  _mapper.Map<IEnumerable<AmenityDto>>(await _amenityRepository.GetAll()) ;
         if (result == null || !result.Any())
         {
             return ApiResponse<IEnumerable<AmenityDto>>.Fail("Không có tiện ích nào.");
         }
           return ApiResponse<IEnumerable<AmenityDto>>.Success(result);
      }
      public IQueryable<AmenityDto> GetAllOdata()
      {
          var amenity = _amenityRepository.GetAllOdata();
      
          return amenity.ProjectTo<AmenityDto>(_mapper.ConfigurationProvider);
      }
    public IQueryable<AmenityDto> GetAllByStaffIdOdata(Guid staffId)
    {
         
        var amenity =   _amenityRepository.GetAllOdata().Where(x=> x.StaffId == staffId);
      
        return amenity.ProjectTo<AmenityDto>(_mapper.ConfigurationProvider);
    }
    // public IQueryable<AmenityDto> GetAllByStaffIdOdata(Guid staffId)
    // {
    //     var staffIdStr = GetStaffIdFromToken();
    //     var amenity =   _amenityRepository.GetAllOdata().Where(x=> x.StaffId == staffId);
    //   
    //     return amenity.ProjectTo<AmenityDto>(_mapper.ConfigurationProvider);
    // }

    public async Task<AmenityDto> GetByIdAsync(Guid id)
    {
        var amenity = await _amenityRepository.GetByIdAsync(id);
        if (amenity == null)
            throw new KeyNotFoundException("AmentityId not found");
      return   _mapper.Map<AmenityDto>(amenity);
    }

    public async  Task<ApiResponse<AmenityDto>> AddAsync(Guid staffId, CreateAmenityDto request )
    { 
       
       
        var exist = await _amenityRepository.AmenityNameExistsAsync(request.AmenityName);
        if (exist)
            return ApiResponse<AmenityDto>.Fail("Tiện ích đã có rồi.");
        
        var amenity = _mapper.Map<Amenity>(request);
        
        if (amenity.StaffId != staffId)
            return ApiResponse<AmenityDto>.Fail("Bạn không có quyền cập nhật Amenity này");

        amenity.CreatedAt = DateTime.UtcNow;
        amenity.StaffId = staffId;
        await _amenityRepository.AddAsync(amenity);
        var result =_mapper.Map<AmenityDto>(amenity);
        return  ApiResponse<AmenityDto>.Success(result,"Thêm tiện ích thành công");
    }

    public async Task<ApiResponse<bool>> UpdateAsync(Guid accountId,Guid id, UpdateAmenityDto request)
    {
        var amenity =await _amenityRepository.GetByIdAsync(id);
        if (amenity == null)
            throw new KeyNotFoundException("AmentityId not found");
       
        if (amenity.StaffId != accountId)
            return ApiResponse<bool>.Fail("Bạn không có quyền cập nhật Amenity này");

        var existAmentityName = await _amenityRepository.AmenityNameExistsAsync(request.AmenityName);
        if(existAmentityName)
            return ApiResponse<bool>.Fail("Tiện ích đã có rồi.");
           // throw new Exception("Tiện ích đã có tại trong nhà trọ.");
         _mapper.Map(request, amenity);
         amenity.UpdatedAt = DateTime.UtcNow;
         await _amenityRepository.UpdateAsync(amenity);
        var result = _mapper.Map<AmenityDto>(amenity);
        return ApiResponse<bool>.Success(true,"Cập nhật tiện ích thành công");
    }
    public async Task DeleteAsync(Guid accountId,Guid id)
    {
        var amenity = await _amenityRepository.GetByIdAsync(id);
        
        if (amenity==null) 
            throw new KeyNotFoundException("AmentityId not found");
        
        // if (amenity.StaffId != accountId)
        //     return ApiResponse<bool>.Fail("Bạn không có quyền cập nhật Amenity này");

        await _amenityRepository.DeleteAsync(amenity);
    }
}