using System.Text.Json;
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
        private readonly HttpClient _httpClient;

        public HouseLocationService(IHouseLocationRepository houseLocationRepo, IMapper mapper,
            HttpClient httpClient)
        {
            _houseLocationRepo = houseLocationRepo;
            _mapper = mapper;
            _httpClient = httpClient;
        }

        private async Task<string?> GetProvinceNameAsync(string provinceId)
        {
            var response = await _httpClient.GetFromJsonAsync<JsonElement>("/api/provinces");
            var provinces = response.GetProperty("provinces").EnumerateArray();
            return provinces.FirstOrDefault(p => p.GetProperty("code").GetString() == provinceId)
                            .GetProperty("name").GetString();
        }

        private async Task<string?> GetCommuneNameAsync(string provinceId, string communeId)
        {
            var response = await _httpClient.GetFromJsonAsync<JsonElement>($"/api/provinces/{provinceId}/communes");
            var communes = response.GetProperty("communes").EnumerateArray();
            return communes.FirstOrDefault(c => c.GetProperty("code").GetString() == communeId)
                           .GetProperty("name").GetString();
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
            var houseLocation = _mapper.Map<HouseLocation>(createDto);
            
            houseLocation.ProvinceName = await GetProvinceNameAsync(houseLocation.ProvinceId) ?? "";
            houseLocation.CommuneName = await GetCommuneNameAsync(houseLocation.ProvinceId, houseLocation.CommuneId) ?? "";
            
            houseLocation.FullAddress = $"{houseLocation.AddressDetail}, {houseLocation.CommuneName}, {houseLocation.ProvinceName}";

            var existAddress = await _houseLocationRepo.LocationExistsWithHouseIdAsync(houseLocation.HouseId, houseLocation.FullAddress);
            if (existAddress)
                return ApiResponse<HouseLocationDTO>.Fail("Địa chỉ này đã tồn tại cho nhà này.");

            await _houseLocationRepo.AddAsync(houseLocation);
            return ApiResponse<HouseLocationDTO>.Success(
                _mapper.Map<HouseLocationDTO>(houseLocation), "Thêm địa chỉ nhà thành công");
        }

        public async Task<ApiResponse<bool>> UpdateAsync(Guid id, UpdateHouseLocationDTO updateDto)
        {
            var exist = await _houseLocationRepo.GetByIdAsync(id);
            if (exist == null) throw new KeyNotFoundException("HouseLocationId not found!");            

            _mapper.Map(updateDto, exist);
            exist.ProvinceName = await GetProvinceNameAsync(exist.ProvinceId) ?? "";
            exist.CommuneName = await GetCommuneNameAsync(exist.ProvinceId, exist.CommuneId) ?? "";
            exist.FullAddress = $"{exist.AddressDetail}, {exist.CommuneName}, {exist.ProvinceName}";

            var existAddress = await _houseLocationRepo.LocationExistsWithHouseIdAsync(exist.HouseId, exist.FullAddress);
            if (existAddress)
                return ApiResponse<bool>.Fail("Địa chỉ này đã tồn tại cho nhà này.");

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
