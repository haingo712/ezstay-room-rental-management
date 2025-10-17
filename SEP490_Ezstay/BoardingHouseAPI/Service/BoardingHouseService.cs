using System.Text.Json;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BoardingHouseAPI.DTO.Request;
using BoardingHouseAPI.DTO.Response;
using BoardingHouseAPI.Models;
using BoardingHouseAPI.Repository.Interface;
using BoardingHouseAPI.Service.Interface;

namespace BoardingHouseAPI.Service
{
    public class BoardingHouseService : IBoardingHouseService
    {
        private readonly IBoardingHouseRepository _boardingHouseRepo;        
        private readonly IMapper _mapper;
        private readonly IImageClientService _imageClient; 
        private readonly IRoomClientService _roomClient;
        private readonly HttpClient _http;

        public BoardingHouseService(IBoardingHouseRepository boardingHouseRepo, IMapper mapper, 
            IImageClientService imageClient, IRoomClientService roomClient,IHttpClientFactory factory)
        {
            _boardingHouseRepo = boardingHouseRepo;            
            _mapper = mapper;
            _imageClient = imageClient;
            _roomClient = roomClient;
            _http = factory.CreateClient("Gateway");  
        }

        private async Task<string?> GetProvinceNameAsync(string provinceId)
        {
            var response = await _http.GetFromJsonAsync<JsonElement>("/api/provinces");
            var provinces = response.GetProperty("provinces").EnumerateArray();
            return provinces.FirstOrDefault(p => p.GetProperty("code").GetString() == provinceId)
                            .GetProperty("name").GetString();
        }

        private async Task<string?> GetCommuneNameAsync(string provinceId, string communeId)
        {
            var response = await _http.GetFromJsonAsync<JsonElement>($"/api/provinces/{provinceId}/communes");
            var communes = response.GetProperty("communes").EnumerateArray();
            return communes.FirstOrDefault(c => c.GetProperty("code").GetString() == communeId)
                           .GetProperty("name").GetString();
        }

        public IQueryable<BoardingHouseDTO> GetAll()
        {
            var houses = _boardingHouseRepo.GetAll();
            return houses.ProjectTo<BoardingHouseDTO>(_mapper.ConfigurationProvider);
        }

        public IQueryable<BoardingHouseDTO> GetByOwnerId(Guid ownerId)
        {
            return _boardingHouseRepo.GetBoardingHousesByOwnerId(ownerId)
                .ProjectTo<BoardingHouseDTO>(_mapper.ConfigurationProvider);
        }      

        public async Task<BoardingHouseDTO?> GetByIdAsync(Guid id)
        {
            var house = await _boardingHouseRepo.GetByIdAsync(id);
            return house == null ? throw new KeyNotFoundException("HouseId not found!")  : 
                _mapper.Map<BoardingHouseDTO>(house);
        }

        public async Task<ApiResponse<BoardingHouseDTO>> CreateAsync(Guid ownerId, CreateBoardingHouseDTO createDto)
        {

            var houseLocation = _mapper.Map<HouseLocation>(createDto.Location);

            houseLocation.ProvinceName = await GetProvinceNameAsync(houseLocation.ProvinceId) ?? "";            
            houseLocation.CommuneName = await GetCommuneNameAsync(houseLocation.ProvinceId, houseLocation.CommuneId) ?? "";

            houseLocation.FullAddress = $"{houseLocation.AddressDetail}, {houseLocation.CommuneName}, {houseLocation.ProvinceName}";

            // check trùng địa chỉ
            var locationExist = await _boardingHouseRepo.LocationExists(houseLocation.FullAddress);
            if (locationExist)
                return ApiResponse<BoardingHouseDTO>.Fail("Địa chỉ này đã tồn tại.");

            //var exist = await _boardingHouseRepo.LocationExistsWithHouseName(createDto.HouseName, houseLocation.FullAddress);
            //if (exist)
            //    return ApiResponse<BoardingHouseDTO>.Fail("Nhà trọ với tên và địa chỉ này đã tồn tại.");
            var house = _mapper.Map<BoardingHouse>(createDto);
            house.OwnerId = ownerId; 
            house.ImageUrl = await _imageClient.UploadImageAsync(createDto.ImageUrl!);
            house.Location = houseLocation;

            await _boardingHouseRepo.AddAsync(house);
            return ApiResponse<BoardingHouseDTO>.Success(
                _mapper.Map<BoardingHouseDTO>(house), "Thêm nhà trọ thành công");
        }

        public async Task<ApiResponse<bool>> UpdateAsync(Guid id, UpdateBoardingHouseDTO updateDto)
        {            
            var exist = await _boardingHouseRepo.GetByIdAsync(id);
            if (exist == null) throw new KeyNotFoundException("HouseId not found!");            
            var oldAddress = exist.Location?.FullAddress ?? "";
            _mapper.Map(updateDto, exist);
            if (exist.Location != null && updateDto.Location != null)
            {
                _mapper.Map(updateDto.Location, exist.Location);
                
                exist.Location.ProvinceName = await GetProvinceNameAsync(exist.Location.ProvinceId) ?? "";
                exist.Location.CommuneName = await GetCommuneNameAsync(exist.Location.ProvinceId, exist.Location.CommuneId) ?? "";
                
                exist.Location.FullAddress =
                    $"{exist.Location.AddressDetail}, {exist.Location.CommuneName}, {exist.Location.ProvinceName}";

                // check trùng địa chỉ (nếu địa chỉ thay đổi)
                var existLocation = await _boardingHouseRepo.LocationExists(exist.Location.FullAddress);
                if (existLocation && exist.Location.FullAddress != oldAddress)
                    return ApiResponse<bool>.Fail("Địa chỉ này đã tồn tại.");

                // Check trùng địa chỉ + tên
                //var existAddress = await _boardingHouseRepo.LocationExistsWithHouseName(updateDto.HouseName, exist.Location.FullAddress);
                //if (existAddress && (exist.HouseName != updateDto.HouseName || exist.Location.FullAddress != oldAddress))
                //    return ApiResponse<bool>.Fail("Nhà trọ với tên và địa chỉ này đã tồn tại.");
            }
            exist.ImageUrl = await _imageClient.UploadImageAsync(updateDto.ImageUrl!);
            await _boardingHouseRepo.UpdateAsync(exist);
            return ApiResponse<bool>.Success(true, "Cập nhật trọ thành công");
        }        

        public async Task<ApiResponse<bool>> DeleteAsync(Guid id)
        {
            var house = await _boardingHouseRepo.GetByIdAsync(id);
            if (house == null) throw new KeyNotFoundException("HouseId not found!");          
            var rooms = await _roomClient.GetRoomsByHouseIdAsync(id);
            if (rooms != null && rooms.Count > 0)
                return ApiResponse<bool>.Fail("Không thể xoá nhà trọ khi còn tồn tại phòng!");


            await _boardingHouseRepo.DeleteAsync(house);
            return ApiResponse<bool>.Success(true, "Xoá trọ thành công");
        }
    }
}
