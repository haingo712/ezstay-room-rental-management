using System.Security.Claims;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BoardingHouseAPI.DTO.Request;
using BoardingHouseAPI.DTO.Response;
using BoardingHouseAPI.Models;
using BoardingHouseAPI.Repository.Interface;
using BoardingHouseAPI.Service.Interface;
using System.Text.Json;

namespace BoardingHouseAPI.Service
{
    public class BoardingHouseService : IBoardingHouseService
    {
        private readonly IBoardingHouseRepository _boardingHouseRepo;        
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _http;

        public BoardingHouseService(IBoardingHouseRepository boardingHouseRepo, IMapper mapper, 
            IHttpContextAccessor httpContextAccessor, IHttpClientFactory factory)
        {
            _boardingHouseRepo = boardingHouseRepo;            
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _http = factory.CreateClient("RoomAPI");
        }

        private Guid GetOwnerIdFromToken()
        {
            var ownerIdStr = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(ownerIdStr))
                throw new UnauthorizedAccessException("Không xác định được OwnerId từ token.");
            return Guid.Parse(ownerIdStr);
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
        public IQueryable<BoardingHouseDTO> GetByOwnerId()
        {
            var ownerId = GetOwnerIdFromToken();
            return _boardingHouseRepo.GetBoardingHousesByOwnerId(ownerId)
                .ProjectTo<BoardingHouseDTO>(_mapper.ConfigurationProvider);
        }

        public async Task<BoardingHouseDTO?> GetByIdAsync(Guid id)
        {
            var house = await _boardingHouseRepo.GetByIdAsync(id);
            return house == null ? throw new KeyNotFoundException("HouseId not found!")  : 
                _mapper.Map<BoardingHouseDTO>(house);
        }

        public async Task<ApiResponse<BoardingHouseDTO>> CreateAsync(CreateBoardingHouseDTO createDto)
        {
            var ownerId = GetOwnerIdFromToken();
            createDto.OwnerId = ownerId;
            var exist = await _boardingHouseRepo.BoardingHouseExistsByOwnerAndNameAsync(createDto.OwnerId, createDto.HouseName);
            if (exist)
                return ApiResponse<BoardingHouseDTO>.Fail("Bạn đã có nhà trọ với tên này rồi.");
            var house = _mapper.Map<BoardingHouse>(createDto);
            house.CreatedAt = DateTime.UtcNow;            

            await _boardingHouseRepo.AddAsync(house);
            return ApiResponse<BoardingHouseDTO>.Success(
                _mapper.Map<BoardingHouseDTO>(house), "Thêm nhà trọ thành công");
        }

        public async Task<ApiResponse<bool>> UpdateAsync(Guid id, UpdateBoardingHouseDTO updateDto)
        {           
            var exist = await _boardingHouseRepo.GetByIdAsync(id);
            if (exist == null) throw new KeyNotFoundException("HouseId not found!");

            var existName = await _boardingHouseRepo.BoardingHouseExistsByOwnerAndNameAsync(exist.OwnerId, updateDto.HouseName);
            if (existName && exist.HouseName != updateDto.HouseName)
                return ApiResponse<bool>.Fail("Bạn đã có nhà trọ với tên này rồi.");

            _mapper.Map(updateDto, exist);
            await _boardingHouseRepo.UpdateAsync(exist);
            return ApiResponse<bool>.Success(true, "Cập nhật trọ thành công");
        }

        /*public async Task<ApiResponse<bool>> DeleteAsync(Guid id)
        {
            var house = await _boardingHouseRepo.GetByIdAsync(id);
            if (house == null) throw new KeyNotFoundException("HouseId not found!");

            await _boardingHouseRepo.DeleteAsync(house);
            return ApiResponse<bool>.Success(true, "Xoá trọ thành công");
        }*/

        public async Task<ApiResponse<bool>> DeleteAsync(Guid id)
        {
            var house = await _boardingHouseRepo.GetByIdAsync(id);
            if (house == null) throw new KeyNotFoundException("HouseId not found!");
          
            var response = await _http.GetAsync($"/api/Rooms/ByHouseId/{id}");
            if (!response.IsSuccessStatusCode)
                return ApiResponse<bool>.Fail("Không thể kiểm tra phòng trong nhà trọ!");

            var roomsJson = await response.Content.ReadAsStringAsync();
            // Nếu RoomAPI trả về danh sách phòng dạng JSON array
            var rooms = JsonSerializer.Deserialize<List<object>>(roomsJson);

            if (rooms != null && rooms.Count > 0)
                return ApiResponse<bool>.Fail("Không thể xoá nhà trọ khi còn tồn tại phòng!");

            await _boardingHouseRepo.DeleteAsync(house);
            return ApiResponse<bool>.Success(true, "Xoá trọ thành công");
        }
    }
}
