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
    
     // public async Task<IEnumerable<RentalRequestDto>> GetAllById(Guid staffId)
     //  {
     //       var amenity =  await _rentalRequestRepository.GetAllByStaffId(staffId);
     //  return _mapper.Map<IEnumerable<RentalRequestDto>>(amenity);
     //  }
  
      // public async Task<ApiResponse<IEnumerable<RentalRequestDto>>> GetAll()
      // {
      //    var result =  _mapper.Map<IEnumerable<RentalRequestDto>>(await _rentalRequestRepository.GetAll()) ;
      //    if (result == null || !result.Any())
      //    {
      //        return ApiResponse<IEnumerable<RentalRequestDto>>.Fail("Không có tiện ích nào.");
      //    }
      //      return ApiResponse<IEnumerable<RentalRequestDto>>.Success(result);
      // }
    // public IQueryable<RentalRequestDto> GetAllByOwnerIdOdata(Guid ownerId, int pageNumber, int pageSize)
    // {
    //     var query = _rentalRequestRepository.GetAllByOwnerIdOdata(ownerId);
    //
    //     // áp dụng phân trang
    //     query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
    //
    //     return query.ProjectTo<RentalRequestDto>(_mapper.ConfigurationProvider);
    // }
    public IQueryable<RentalRequestDto> GetAllByOwnerIdOdata(Guid ownerId)
    {
        var rental =   _rentalRequestRepository.GetAllByOwnerIdOdata(ownerId);
        return rental.ProjectTo<RentalRequestDto>(_mapper.ConfigurationProvider);
    }

    public IQueryable<RentalRequestDto> GetAllByUserIdOdata(Guid userId)
    {
        var rental =   _rentalRequestRepository.GetAllByUserIdOdata(userId);
        return rental.ProjectTo<RentalRequestDto>(_mapper.ConfigurationProvider);
    }

    public async Task<RentalRequestDto> GetByIdAsync(Guid id)
    {
        var rental = await _rentalRequestRepository.GetByIdAsync(id);
        if (rental == null)
            throw new KeyNotFoundException("Rental Request Id not found");
      return   _mapper.Map<RentalRequestDto>(rental);
    }

    public async  Task<ApiResponse<RentalRequestDto>> AddAsync(CreateRentalRequestDto request)
    { 
        // var exist = await _rentalRequestRepository.AmenityNameExistsAsync(request.AmenityName);
        // if (exist)
        //     return ApiResponse<RentalRequestDto>.Fail("Tiện ích đã có rồi.");
        var rentalRequest = _mapper.Map<RentalRequest>(request);
        await _rentalRequestRepository.AddAsync(rentalRequest);
        var result =_mapper.Map<RentalRequestDto>(rentalRequest);
        return  ApiResponse<RentalRequestDto>.Success(result,"Thêm Rental Request thành công");
    }

    public async Task<ApiResponse<bool>> UpdateAsync(Guid id, UpdateRentalRequestDto request)
    {
        var rentalRequest =await _rentalRequestRepository.GetByIdAsync(id);
        // if (amenity == null)
        //     throw new KeyNotFoundException("AmentityId not found");
        // var existAmentityName = await _rentalRequestRepository.AmenityNameExistsAsync(request.AmenityName);
   //     if(existAmentityName)
      //      return ApiResponse<bool>.Fail("Tiện ích đã có rồi.");
           // throw new Exception("Tiện ích đã có tại trong nhà trọ.");
         _mapper.Map(request, rentalRequest);
         await _rentalRequestRepository.UpdateAsync(rentalRequest);
        var result = _mapper.Map<RentalRequestDto>(rentalRequest);
        return ApiResponse<bool>.Success(true,"Cập nhật Rental Request thành công");
    }
    // public async Task DeleteAsync(Guid id)
    // {
    //     var amenity = await _rentalRequestRepository.GetByIdAsync(id);
    //     if (amenity==null) 
    //         throw new KeyNotFoundException("AmentityId not found");
    //     await _rentalRequestRepository.DeleteAsync(amenity);
    // }
}