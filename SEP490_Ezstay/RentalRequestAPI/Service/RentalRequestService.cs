using AutoMapper;
using AutoMapper.QueryableExtensions;
using RentalRequestAPI.DTO.Request;
using RentalRequestAPI.DTO.Response;
using RentalRequestAPI.Model;
using RentalRequestAPI.Repository.Interface;
using RentalRequestAPI.Service.Interface;

namespace RentalRequestAPI.Service;

public class RentalRequestService: IRentalRequestService
{
    private readonly IMapper _mapper;
    private readonly IRentalRequestRepository _rentalRequestRepository;
    private readonly HttpClient _httpClient;
    public RentalRequestService(IMapper mapper, IRentalRequestRepository rentalRequestRepository,  HttpClient httpClient)
    {
        _mapper = mapper;
        _rentalRequestRepository = rentalRequestRepository;
        _httpClient = httpClient;
    }
    
     public async Task<IEnumerable<RentalRequestDto>> GetAllByStaffId(Guid staffId)
      {
           var amenity =  await _rentalRequestRepository.GetAllByStaffId(staffId);
      return _mapper.Map<IEnumerable<RentalRequestDto>>(amenity);
      }
  
      public async Task<ApiResponse<IEnumerable<RentalRequestDto>>> GetAll()
      {
         var result =  _mapper.Map<IEnumerable<RentalRequestDto>>(await _rentalRequestRepository.GetAll()) ;
         if (result == null || !result.Any())
         {
             return ApiResponse<IEnumerable<RentalRequestDto>>.Fail("Không có tiện ích nào.");
         }
           return ApiResponse<IEnumerable<RentalRequestDto>>.Success(result);
      }
      public IQueryable<RentalRequestDto> GetAllOdata()
      {
          var amenity = _rentalRequestRepository.GetAllOdata();
      
          return amenity.ProjectTo<RentalRequestDto>(_mapper.ConfigurationProvider);
      }
    public IQueryable<RentalRequestDto> GetAllByStaffIdOdata(Guid staffId)
    {
        var amenity =   _rentalRequestRepository.GetAllOdata().Where(x=> x.UserId == staffId);
      
        return amenity.ProjectTo<RentalRequestDto>(_mapper.ConfigurationProvider);
    }

    public async Task<RentalRequestDto> GetByIdAsync(Guid id)
    {
        var amenity = await _rentalRequestRepository.GetByIdAsync(id);
        if (amenity == null)
            throw new KeyNotFoundException("AmentityId not found");
      return   _mapper.Map<RentalRequestDto>(amenity);
    }

    public async  Task<ApiResponse<RentalRequestDto>> AddAsync(CreateRentalRequestDto request)
    { 
        // var exist = await _rentalRequestRepository.AmenityNameExistsAsync(request.AmenityName);
        // if (exist)
        //     return ApiResponse<RentalRequestDto>.Fail("Tiện ích đã có rồi.");
        var amenity = _mapper.Map<RentalRequest>(request);
        await _rentalRequestRepository.AddAsync(amenity);
        var result =_mapper.Map<RentalRequestDto>(amenity);
        return  ApiResponse<RentalRequestDto>.Success(result,"Thêm tiện ích thành công");
    }

    public async Task<ApiResponse<bool>> UpdateAsync(Guid id, UpdateRentalRequestDto request)
    {
        var amenity =await _rentalRequestRepository.GetByIdAsync(id);
        // if (amenity == null)
        //     throw new KeyNotFoundException("AmentityId not found");
        // var existAmentityName = await _rentalRequestRepository.AmenityNameExistsAsync(request.AmenityName);
   //     if(existAmentityName)
      //      return ApiResponse<bool>.Fail("Tiện ích đã có rồi.");
           // throw new Exception("Tiện ích đã có tại trong nhà trọ.");
         _mapper.Map(request, amenity);
         await _rentalRequestRepository.UpdateAsync(amenity);
        var result = _mapper.Map<RentalRequestDto>(amenity);
        return ApiResponse<bool>.Success(true,"Cập nhật tiện ích thành công");
    }
    public async Task DeleteAsync(Guid id)
    {
        var amenity = await _rentalRequestRepository.GetByIdAsync(id);
        if (amenity==null) 
            throw new KeyNotFoundException("AmentityId not found");
        await _rentalRequestRepository.DeleteAsync(amenity);
    }
}