using AutoMapper;
using AutoMapper.QueryableExtensions;
using HouseLocationAPI.DTO.Request;
using HouseLocationAPI.DTO.Response;
using HouseLocationAPI.Models;
using HouseLocationAPI.Repository.Interface;
using HouseLocationAPI.Service.Interface;

namespace HouseLocationAPI.Service
{
    public class HouseLocationService : IHouseLocationService
    {
        private readonly IHouseLocationRepository _houseLocationRepo;
        private readonly IMapper _mapper;

        public HouseLocationService(IHouseLocationRepository houseLocationRepo, IMapper mapper)
        {
            _houseLocationRepo = houseLocationRepo;
            _mapper = mapper;
        }       

        public IQueryable<HouseLocationDTO> GetAll()
        {
            var hl = _houseLocationRepo.GetAll();
            return hl.ProjectTo<HouseLocationDTO>(_mapper.ConfigurationProvider);
        }

        public IQueryable<HouseLocationDTO> GetByHouseId(Guid houseId)
        {
            return _houseLocationRepo.GetHouseLocationsByHouseId(houseId)
                .ProjectTo<HouseLocationDTO>(_mapper.ConfigurationProvider);
        }

        public async Task<HouseLocationDTO?> GetByIdAsync(Guid id)
        {
            var hl = await _houseLocationRepo.GetByIdAsync(id);
            return hl == null ? null : _mapper.Map<HouseLocationDTO>(hl);
        }

        public async Task<ApiResponse<HouseLocationDTO>> CreateAsync(CreateHouseLocationDTO createDto)
        {
            var exist = await _houseLocationRepo.LocationExistsWithHouseIdAsync(createDto.HouseId, createDto.FullAddress); 
            if (exist)
                return ApiResponse<HouseLocationDTO>.Fail("Địa chỉ này đã tồn tại cho nhà này.");
            var houseLocation = _mapper.Map<HouseLocation>(createDto);            

            await _houseLocationRepo.AddAsync(houseLocation);
            return ApiResponse<HouseLocationDTO>.Success(
                _mapper.Map<HouseLocationDTO>(houseLocation), "Thêm địa chỉ nhà thành công");
        }

        public async Task<ApiResponse<bool>> UpdateAsync(Guid id, UpdateHouseLocationDTO updateDto)
        {
            var exist = await _houseLocationRepo.GetByIdAsync(id);
            if (exist == null) throw new KeyNotFoundException("HouseLocationId not found!");

            var existAddress = await _houseLocationRepo.LocationExistsWithHouseIdAsync(exist.HouseId, updateDto.FullAddress);
            if (existAddress && exist.FullAddress != updateDto.FullAddress)
                return ApiResponse<bool>.Fail("Địa chỉ này đã tồn tại cho nhà này.");

            _mapper.Map(updateDto, exist);
            await _houseLocationRepo.UpdateAsync(exist);
            return ApiResponse<bool>.Success(true, "Cập nhật địa chỉ nhà thành công");
        }

        public async Task<ApiResponse<bool>> DeleteAsync(Guid id)
        {
            var houseLocation = await _houseLocationRepo.GetByIdAsync(id);
            if (houseLocation == null) throw new KeyNotFoundException("HouseLocationId not found!");

            await _houseLocationRepo.DeleteAsync(houseLocation);
            return ApiResponse<bool>.Success(true, "Xoá địa chỉ nhà thành công");
        }
    }
}
