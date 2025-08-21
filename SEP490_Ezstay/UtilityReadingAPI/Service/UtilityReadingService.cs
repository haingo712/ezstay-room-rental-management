using AutoMapper;
using UtilityReadingAPI.DTO.Request;
using UtilityReadingAPI.DTO.Response;
using UtilityReadingAPI.Model;
using UtilityReadingAPI.Repository.Interface;
using UtilityReadingAPI.Service.Interface;

namespace UtilityReadingAPI.Service;

public class UtilityReadingService: IUtilityReadingService
{
    private readonly IMapper _mapper;
    private readonly IUtilityReadingRepository _utilityReadingRepository;

    public UtilityReadingService(IMapper mapper, IUtilityReadingRepository utilityReadingRepository)
    {
        _mapper = mapper;
        _utilityReadingRepository= utilityReadingRepository;
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

    public async Task<UtilityReadingDto> GetByIdAsync(Guid id)
    {
        var utilityReading = await _utilityReadingRepository.GetByIdAsync(id);
        if (utilityReading==null)
            throw new KeyNotFoundException(" UtilityReading Id not found");
         return   _mapper.Map<UtilityReadingDto>(utilityReading);
    }

    public async  Task<ApiResponse<UtilityReadingDto>> AddAsync(CreateUtilityReadingDto request)
    { 
         if (await _utilityReadingRepository.ExistsUtilityReadingInMonthAsync(request.RoomId, request.Type, request.ReadingDate)) 
             return  ApiResponse<UtilityReadingDto>.Fail("Đã tồn tại chỉ số cho phòng này trong tháng này.");
        var utilityReading = _mapper.Map<UtilityReading>(request);
       
        await _utilityReadingRepository.AddAsync(utilityReading);
        var result =_mapper.Map<UtilityReadingDto>(utilityReading);
        return  ApiResponse<UtilityReadingDto>.Success(result,"Thêm"+request.Type+" thành công");
    }

    public async Task<ApiResponse<bool>> UpdateAsync(Guid id, UpdateUtilityReadingDto request)
    {
        var utilityReading =await _utilityReadingRepository.GetByIdAsync(id);
        if (utilityReading==null)
            throw new KeyNotFoundException("Id not found");
        if (request.CurrentIndex < utilityReading.PreviousIndex)
        {
            return ApiResponse<bool>.Fail("CurrentIndex k dc nho hon PreviousIndex");
        }
         _mapper.Map(request,utilityReading);
         await _utilityReadingRepository.UpdateAsync(utilityReading);
        var result = _mapper.Map<UtilityReadingDto>(utilityReading);
        return ApiResponse<bool>.Success(true,"Cập nhật thành công");
    }
    // public async Task DeleteAsync(Guid id)
    // {
    //     var amenity = await _amenityRepository.GetByIdAsync(id);
    //     if (amenity==null) 
    //         throw new KeyNotFoundException("k tim thay phong tro");
    //     await _amenityRepository.DeleteAsync(amenity);
    // }
}