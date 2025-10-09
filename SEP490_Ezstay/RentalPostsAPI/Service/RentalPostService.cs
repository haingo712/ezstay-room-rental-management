using AutoMapper;
using AutoMapper.QueryableExtensions;
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
        public IQueryable<RentalpostDTO> GetAllAsQueryable()
        {
            var entity = _repo.GetAllAsQueryable();
            return entity.ProjectTo<RentalpostDTO>(_mapper.ConfigurationProvider);
        }

        public async Task<ApiResponse<RentalpostDTO>> CreateAsync(CreateRentalPostDTO dto, ClaimsPrincipal user)
        {
           
            var ownerId = _tokenService.GetUserIdFromClaims(user);
            if (ownerId == Guid.Empty)
                return ApiResponse<RentalpostDTO>.Fail("Không xác định được người dùng");

  
            var house = await _externalService.GetBoardingHouseByIdAsync(dto.BoardingHouseId);
            if (house == null)
                return ApiResponse<RentalpostDTO>.Fail("Không tìm thấy trọ");

          
            if (house.OwnerId != ownerId)
                return ApiResponse<RentalpostDTO>.Fail("Bạn không có quyền đăng bài cho trọ này");

      
            List<RoomDto>? selectedRooms = new();
            if (dto.RoomId != null && dto.RoomId.Any())
            {
                var allRooms = await _externalService.GetRoomsByBoardingHouseIdAsync(dto.BoardingHouseId);
                selectedRooms = allRooms.Where(r => dto.RoomId.Contains(r.Id)).ToList();

                if (selectedRooms.Count != dto.RoomId.Count)
                    return ApiResponse<RentalpostDTO>.Fail("Một số phòng không thuộc trọ đã chọn");
            }

         
            List<string>? imageUrls = null;
            if (dto.Images != null && dto.Images.Any())
            {
                imageUrls = await _externalService.UploadImagesAsync(dto.Images);
            }

           
            var entity = _mapper.Map<RentalPosts>(dto);
            entity.Id = Guid.NewGuid();
            entity.AuthorId = ownerId;
            entity.CreatedAt = entity.UpdatedAt = DateTime.UtcNow;
            entity.IsActive = true;
            entity.IsApproved = 0;
            entity.ImageUrls = imageUrls ?? new List<string>();

       
            await _repo.CreateAsync(entity);

         
            var result = _mapper.Map<RentalpostDTO>(entity);
            result.AuthorName = _tokenService.GetFullNameFromClaims(user) ?? "Unknown";
            result.ContactPhone = dto.ContactPhone;
            result.HouseName = house.HouseName;
            result.RoomName = (selectedRooms != null && selectedRooms.Any())
                ? string.Join(", ", selectedRooms.Select(r => r.RoomName))
                : "Toàn khu trọ";
            result.ImageUrls = entity.ImageUrls;

            return ApiResponse<RentalpostDTO>.Success(result, "Tạo bài đăng thành công");
        }


        /*  public async Task<IEnumerable<RentalpostDTO>> GetAllForUserAsync()
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
          }*/
        public async Task<IEnumerable<RentalpostDTO>> GetAllForUserAsync()
        {
            // Chỉ lấy IsActive = true
            var entities = (await _repo.GetAllAsync()).Where(x => x.IsActive).ToList();
            return _mapper.Map<List<RentalpostDTO>>(entities);
        }
        public async Task<IEnumerable<RentalpostDTO>> GetAllForOwnerAsync(ClaimsPrincipal user)
        {
            var ownerId = _tokenService.GetUserIdFromClaims(user);
            var entities = await _repo.GetAllByOwnerIdAsync(ownerId);
            var dtos = _mapper.Map<List<RentalpostDTO>>(entities);

            // Gắn thông tin từ token (đã có sẵn trong claim)
            foreach (var dto in dtos)
            {
                dto.AuthorName = _tokenService.GetFullNameFromClaims(user) ?? "Unknown";
                dto.ContactPhone = _tokenService.GetPhoneFromClaims(user) ?? "";
            }

            return dtos;
        }


        /* public async Task<IEnumerable<RentalpostDTO>> GetAllForOwnerAsync(ClaimsPrincipal user)
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
         }*/
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

        public async Task<Guid?> GetPostIdByRoomIdAsync(Guid roomId)
        {
            return await _repo.GetPostIdByRoomIdAsync(roomId);
        }
        // public async Task<IEnumerable<RentalpostDTO>> GetByRoomIdAsync(Guid roomId)
        // {
        //     var entity= await _repo.GetByRoomIdAsync(roomId);
        //     return   _mapper.Map<IEnumerable<RentalpostDTO>>(entity);
        // }   
       
    }
}
