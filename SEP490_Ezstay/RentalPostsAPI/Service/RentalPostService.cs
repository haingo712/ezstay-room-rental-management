using AutoMapper;
using RentalPostsAPI.DTO.Request;
using RentalPostsAPI.DTO.Response;
using RentalPostsAPI.Models;
using RentalPostsAPI.Repository.Interface;
using RentalPostsAPI.Service.Interface;
using System.Security.Claims;
using ZstdSharp.Unsafe;

namespace RentalPostsAPI.Service
{

    public class RentalPostService : IRentalPostService
    {
        private readonly IRentalPostRepository _repo;
        private readonly IMapper _mapper;
        private readonly ExternalService _externalService;
        private readonly ITokenService _tokenService;
        public RentalPostService(IRentalPostRepository repo, IMapper mapper, ExternalService externalService, ITokenService tokenService)
        {
            _repo = repo;
            _mapper = mapper;
            _externalService = externalService;
            _tokenService = tokenService;
        }

        public async Task<ApiResponse<RentalpostDTO>> CreateAsync(
            CreateRentalPostDTO dto, ClaimsPrincipal user)
        {
            // 1. Lấy ownerId từ token
            var ownerId = _tokenService.GetUserIdFromClaims(user);
          
            // 2. Lấy room
            var room = await _externalService.GetRoomByIdAsync(dto.RoomId);
            if (room == null)
                return ApiResponse<RentalpostDTO>.Fail("Room not found");

            // 3. Lấy boarding house từ room.HouseId
            var house = await _externalService.GetBoardingHouseByIdAsync(room.HouseId);
            if (house == null)
                return ApiResponse<RentalpostDTO>.Fail("Boarding house not found");

            // 4. Check quyền: house.OwnerId phải bằng ownerId trong token
            if (house.OwnerId != ownerId)
                return ApiResponse<RentalpostDTO>.Fail("You are not the owner of this house");

            // 5. Map DTO → Entity
            var entity = _mapper.Map<RentalPosts>(dto);
            entity.Id = Guid.NewGuid();
            entity.AuthorId = ownerId;
            entity.CreatedAt = entity.UpdatedAt = DateTime.UtcNow;
            entity.IsActive = true;
            entity.IsApproved = 0;

            // 6. Lưu vào DB
            await _repo.CreateAsync(entity);

            // 7. Tạo DTO trả về (không dùng AutoMapper nữa)
            var result = new RentalpostDTO
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                IsActive = entity.IsActive,
                IsApproved = entity.IsApproved,

                AuthorName = _tokenService.GetFullNameFromClaims(user) ?? "Unknown",
                ContactPhone = _tokenService.GetPhoneFromClaims(user) ?? "",
                HouseName = house.HouseName,
                RoomName = room.RoomName
            };

            // 8. Trả về
            return ApiResponse<RentalpostDTO>.Success(result, "Post created successfully");
        }


        public async Task<IEnumerable<RentalpostDTO>> GetAllForUserAsync()
        {
            var entities = await _repo.GetAllAsync();
            var dtos = _mapper.Map<List<RentalpostDTO>>(entities);

            for (int i = 0; i < entities.Count(); i++)
            {
                var entity = entities.ElementAt(i);
                var dto = dtos[i];

                // lấy thông tin chủ bài đăng
                var owner = await _externalService.GetAccountByIdAsync(entity.AuthorId);
         

                var room = await _externalService.GetRoomByIdAsync(entity.RoomId);
                var house = room != null
                    ? await _externalService.GetBoardingHouseByIdAsync(room.HouseId)
                    : null;

                dto.AuthorName = owner?.FullName ?? "Unknown";
                dto.ContactPhone = owner?.Phone ?? "";
                dto.RoomName = room?.RoomName ?? "";
                dto.HouseName = house?.HouseName ?? "";
            }

            return dtos;
        }

    

        // Lấy tất cả bài post của Owner hiện tại
        public async Task<IEnumerable<RentalpostDTO>> GetAllForOwnerAsync(ClaimsPrincipal user)
        {
            var ownerId = _tokenService.GetUserIdFromClaims(user);

            var entities = await _repo.GetAllByOwnerIdAsync(ownerId);
            var dtos = _mapper.Map<List<RentalpostDTO>>(entities);

            for (int i = 0; i < entities.Count(); i++)
            {
                var entity = entities.ElementAt(i);
                var dto = dtos[i];

                var room = await _externalService.GetRoomByIdAsync(entity.RoomId);
                var house = room != null
                    ? await _externalService.GetBoardingHouseByIdAsync(room.HouseId)
                    : null;

                dto.AuthorName = _tokenService.GetFullNameFromClaims(user) ?? "Unknown";
                dto.ContactPhone = _tokenService.GetPhoneFromClaims(user) ?? "";
                dto.RoomName = room?.RoomName ?? "";
                dto.HouseName = house?.HouseName ?? "";
            }

            return dtos;
        }
        public async Task<RentalpostDTO?> GetByIdAsync(Guid id)
        {
            var entity = await _repo.GetByIdAsync(id);
            return entity == null ? null : _mapper.Map<RentalpostDTO>(entity);
        }

        public async Task<RentalpostDTO?> UpdateAsync(Guid id, UpdateRentalPostDTO dto)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return null;

            _mapper.Map(dto, entity);
            entity.UpdatedAt = DateTime.UtcNow;

            var updated = await _repo.UpdateAsync(entity);
            return updated == null ? null : _mapper.Map<RentalpostDTO>(updated);
        }

        public async Task<bool> DeleteAsync(Guid id, Guid deletedBy)
        {
            return await _repo.DeleteAsync(id, deletedBy);
        }

        
    }
}
