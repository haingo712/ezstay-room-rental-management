using AmenityAPI.Service.Interface;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using AmenityAPI.DTO.Request;
using AmenityAPI.DTO.Response;
using AmenityAPI.Models;
using AmenityAPI.Repository.Interface;

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

  // public IQueryable<AmenityDto> GetAllOdata()
  //   {
  //       var amenity =   _amenityRepository.GetAll();
  //   return amenity.ProjectTo<AmenityDto>(_mapper.ConfigurationProvider);
  //   }
  public async Task<IEnumerable<AmenityDto>> GetAll()
      {
           var amenity =  await _amenityRepository.GetAll();
      return _mapper.Map<IEnumerable<AmenityDto>>(amenity);
      
}
    public Task<IEnumerable<String>> GetAllDistinctNameAsync()
    {
        return _amenityRepository.GetAllDistinctNameAsync();
    }
    public IQueryable<AmenityDto> GetAllByOwnerId(Guid ownerId)
    {
        var amenity =   _amenityRepository.GetAllOdata().Where(x=> x.OwnerId == ownerId);
      
        return amenity.ProjectTo<AmenityDto>(_mapper.ConfigurationProvider);
    }

    public async Task<AmenityDto> GetByIdAsync(int id)
    {
        var amenity = await _amenityRepository.GetByIdAsync(id);
        if (amenity == null)
            throw new KeyNotFoundException("AmentityId not found");
      return   _mapper.Map<AmenityDto>(amenity);
    }

    public async  Task<ApiResponse<AmenityDto>> AddAsync(CreateAmenityDto request)
    { 
        
        var exist = await _amenityRepository.AmenityNameExistsAsync(request.AmenityName);
        if (exist)
            return ApiResponse<AmenityDto>.Fail("Tiện ích đã có tại trong nhà trọ.");
          //  throw new Exception("Tiện ích đã có tại trong nhà trọ.");
        
        var amenity = _mapper.Map<Amenity>(request);
        await _amenityRepository.AddAsync(amenity);
        var result =_mapper.Map<AmenityDto>(amenity);
        return  ApiResponse<AmenityDto>.Success(result,"Thêm tiện ích thành công");
    }

    public async Task<ApiResponse<bool>> UpdateAsync(int id, UpdateAmenityDto request)
    {
        var amenity =await _amenityRepository.GetByIdAsync(id);
        if (amenity == null)
            throw new KeyNotFoundException("AmentityId not found");
        var existAmentityName = await _amenityRepository.AmenityNameExistsAsync(request.AmenityName);
        if(existAmentityName)
            return ApiResponse<bool>.Fail("Tiện ích đã có tại trong nhà trọ.");
           // throw new Exception("Tiện ích đã có tại trong nhà trọ.");
         _mapper.Map(request, amenity);
         await _amenityRepository.UpdateAsync(amenity);
        var result = _mapper.Map<AmenityDto>(amenity);
        return ApiResponse<bool>.Success(true,"Cập nhật tiện ích thành công");
    }
    public async Task DeleteAsync(int id)
    {
        var amenity = await _amenityRepository.GetByIdAsync(id);
        if (amenity==null) 
            throw new KeyNotFoundException("k tim thay phong tro");
        await _amenityRepository.DeleteAsync(amenity);
    }
}