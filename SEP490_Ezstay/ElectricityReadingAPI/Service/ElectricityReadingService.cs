using AutoMapper;
using ElectricityReadingAPI.DTO.Request;
using ElectricityReadingAPI.DTO.Response;
using ElectricityReadingAPI.Model;
using ElectricityReadingAPI.Repository.Interface;
using ElectricityReadingAPI.Service.Interface;

namespace ElectricityReadingAPI.Service;

public class ElectricityReadingService: IElectricityReadingService
{
    private readonly IMapper _mapper;
    private readonly IElectricityReadingRepository _electricityReadingRepository;

    public ElectricityReadingService(IMapper mapper, IElectricityReadingRepository electricityReadingRepository)
    {
        _mapper = mapper;
        _electricityReadingRepository= electricityReadingRepository;
    }
    
//   public async Task<IEnumerable<ElectricityReadingDto>> GetAllByOwnerId(Guid ownerId)
//       {
//            var amenity =  await _electricityReadingRepository.GetAllByOwnerId(ownerId);
//       return _mapper.Map<IEnumerable<ElectricityReadingDto>>(amenity);
// }
   
    // public IQueryable<ElectricityReadingDto> GetAllByOwnerIdOdata(Guid ownerId)
    // {
    //     var amenity =   _amenityRepository.GetAllOdata().Where(x=> x.OwnerId == ownerId);
    //   
    //     return amenity.ProjectTo<AmenityDto>(_mapper.ConfigurationProvider);
    // }

    public async Task<ElectricityReadingDto> GetByIdAsync(Guid id)
    {
        var electricityReading = await _electricityReadingRepository.GetByIdAsync(id);
        if (electricityReading ==null)
            throw new KeyNotFoundException("electricityReading Id not found");
      return   _mapper.Map<ElectricityReadingDto>(electricityReading);
    }

    public async  Task<ApiResponse<ElectricityReadingDto>> AddAsync(CreateElectricityReadingDto request)
    { 
        // var exist = await _amenityRepository.AmenityNameExistsAsync(request.AmenityName);
        // if (exist)
        //     return ApiResponse<AmenityDto>.Fail("Tiện ích đã có tại trong nhà trọ.");
        var amenity = _mapper.Map<ElectricityReading>(request);
        await _electricityReadingRepository.AddAsync(amenity);
        var result =_mapper.Map<ElectricityReadingDto>(amenity);
        return  ApiResponse<ElectricityReadingDto>.Success(result,"Thêm tiện ích thành công");
    }

    public async Task<ApiResponse<bool>> UpdateAsync(Guid id, UpdateElectricityReadingDto request)
    {
        var amenity =await _electricityReadingRepository.GetByIdAsync(id);
        if (amenity == null)
            throw new KeyNotFoundException("AmentityId not found");
        // var existAmentityName = await _electricityReadingRepository.AmenityNameExistsAsync(request.AmenityName);
        // if(existAmentityName)
        //     return ApiResponse<bool>.Fail("Tiện ích đã có tại trong nhà trọ.");
           // throw new Exception("Tiện ích đã có tại trong nhà trọ.");
         _mapper.Map(request, amenity);
         await _electricityReadingRepository.UpdateAsync(amenity);
        var result = _mapper.Map<ElectricityReadingDto>(amenity);
        return ApiResponse<bool>.Success(true,"Cập nhật tiện ích thành công");
    }
    // public async Task DeleteAsync(Guid id)
    // {
    //     var amenity = await _amenityRepository.GetByIdAsync(id);
    //     if (amenity==null) 
    //         throw new KeyNotFoundException("k tim thay phong tro");
    //     await _amenityRepository.DeleteAsync(amenity);
    // }
}