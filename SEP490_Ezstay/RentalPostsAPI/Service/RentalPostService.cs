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


        /* public async Task<IEnumerable<RentalpostDTO>> GetAllForUserAsync()
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
 */
        public async Task<IEnumerable<RentalpostDTO>> GetAllForUserAsync()
        {
            var entities = (await _repo.GetAllAsync()).ToList();
            var dtos = _mapper.Map<List<RentalpostDTO>>(entities);

            // Lấy toàn bộ AuthorId duy nhất, tránh gọi trùng
            var authorIds = entities.Select(e => e.AuthorId).Distinct().ToList();
            var authorTasks = authorIds.ToDictionary(
                id => id,
                id => _externalService.GetAccountByIdAsync(id)
            );
            await Task.WhenAll(authorTasks.Values);

            for (int i = 0; i < entities.Count; i++)
            {
                var entity = entities[i];
                var dto = dtos[i];

                var owner = await authorTasks[entity.AuthorId];
                dto.AuthorName = owner?.FullName ?? "Unknown";
                dto.ContactPhone = owner?.Phone ?? "";

                // Nếu bài có nhiều phòng
                if (entity.RoomId != null && entity.RoomId.Any())
                {
                    var roomTasks = entity.RoomId
                        .Select(id => _externalService.GetRoomByIdAsync(id))
                        .ToList();

                    var rooms = await Task.WhenAll(roomTasks);
                    var roomNames = rooms
                        .Where(r => r != null)
                        .Select(r => r!.RoomName)
                        .ToList();

                    dto.RoomName = string.Join(", ", roomNames);

                    // Lấy house đầu tiên (nếu có)
                    var firstRoom = rooms.FirstOrDefault(r => r != null);
                    if (firstRoom != null)
                    {
                        var house = await _externalService.GetBoardingHouseByIdAsync(firstRoom.HouseId);
                        dto.HouseName = house?.HouseName ?? "";
                    }
                }
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

        public async Task<IEnumerable<RentalpostDTO>> GetAllForOwnerAsync(ClaimsPrincipal user)
        {
            var ownerId = _tokenService.GetUserIdFromClaims(user);
            var entities = (await _repo.GetAllByOwnerIdAsync(ownerId)).ToList();
            var dtos = _mapper.Map<List<RentalpostDTO>>(entities);

            var authorIds = entities.Select(e => e.AuthorId).Distinct().ToList();
            var authorTasks = authorIds.ToDictionary(
                id => id,
                id => _externalService.GetAccountByIdAsync(id)
            );
            await Task.WhenAll(authorTasks.Values);

            for (int i = 0; i < entities.Count; i++)
            {
                var entity = entities[i];
                var dto = dtos[i];

                var owner = await authorTasks[entity.AuthorId];
                dto.AuthorName = owner?.FullName ?? "Unknown";
                dto.ContactPhone = owner?.Phone ?? "";

                if (entity.RoomId != null && entity.RoomId.Any())
                {
                 
                    var roomTasks = entity.RoomId
                        .Select(id => _externalService.GetRoomByIdAsync(id))
                        .ToList();

                    var rooms = await Task.WhenAll(roomTasks);
                    var roomNames = rooms
                        .Where(r => r != null)
                        .Select(r => r!.RoomName)
                        .ToList();

                    dto.RoomName = string.Join(", ", roomNames);

                  
                    var firstRoom = rooms.FirstOrDefault(r => r != null);
                    if (firstRoom != null)
                    {
                        var house = await _externalService.GetBoardingHouseByIdAsync(firstRoom.HouseId);
                        dto.HouseName = house?.HouseName ?? "";
                    }
                }
            }

            return dtos;
        }
        public async Task<RentalpostDTO?> GetByIdAsync(Guid id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null)
                return null;

            var dto = _mapper.Map<RentalpostDTO>(entity);

          
            var authorTask = _externalService.GetAccountByIdAsync(entity.AuthorId);

            Task<RoomDto?>[]? roomTasks = null;
            if (entity.RoomId != null && entity.RoomId.Any())
            {
                roomTasks = entity.RoomId
                    .Select(rid => _externalService.GetRoomByIdAsync(rid))
                    .ToArray();
            }

            await Task.WhenAll(authorTask, roomTasks != null ? Task.WhenAll(roomTasks) : Task.CompletedTask);

            var author = await authorTask;
            dto.AuthorName = author?.FullName ?? "Unknown";
            dto.ContactPhone = author?.Phone ?? "";


            if (roomTasks != null)
            {
                var rooms = (await Task.WhenAll(roomTasks))
                    .Where(r => r != null)
                    .ToList();

                dto.RoomName = string.Join(", ", rooms.Where(r => r != null).Select(r => r!.RoomName));

                // Gọi house của phòng đầu tiên (nếu có)
                var firstRoom = rooms.FirstOrDefault();
                if (firstRoom != null)
                {
                    var house = await _externalService.GetBoardingHouseByIdAsync(firstRoom.HouseId);
                    dto.HouseName = house?.HouseName ?? "";
                }
            }
            if (entity.RoomId != null && entity.RoomId.Any())
            {
                var reviews = await _externalService.GetReviewsByRoomIdsAsync(entity.RoomId);
                dto.Reviews = reviews;
            }

            dto.ImageUrls = entity.ImageUrls ?? new List<string>();

            return dto;
        }



        public async Task<RentalpostDTO?> UpdateAsync(Guid id, UpdateRentalPostDTO dto)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null)
                return null;

            _mapper.Map(dto, entity);
            entity.UpdatedAt = DateTime.UtcNow;

            var updated = await _repo.UpdateAsync(entity);
            if (updated == null)
                return null;

            var result = _mapper.Map<RentalpostDTO>(updated);

           
            var authorTask = _externalService.GetAccountByIdAsync(entity.AuthorId);

         
            Task<RoomDto?>[]? roomTasks = null;
            if (entity.RoomId != null && entity.RoomId.Any())
            {
                roomTasks = entity.RoomId
                    .Select(rid => _externalService.GetRoomByIdAsync(rid))
                    .ToArray();
            }

            await Task.WhenAll(authorTask, roomTasks != null ? Task.WhenAll(roomTasks) : Task.CompletedTask);

       
            var author = await authorTask;
            result.AuthorName = author?.FullName ?? "Unknown";
            result.ContactPhone = author?.Phone ?? "";

   
            if (roomTasks != null)
            {
                var rooms = (await Task.WhenAll(roomTasks))
                    .Where(r => r != null)
                    .ToList();

                result.RoomName = string.Join(", ", rooms.Select(r => r!.RoomName));

                var firstRoom = rooms.FirstOrDefault();
                if (firstRoom != null)
                {
                    var house = await _externalService.GetBoardingHouseByIdAsync(firstRoom.HouseId);
                    result.HouseName = house?.HouseName ?? "";
                }
            }
            result.ImageUrls = entity.ImageUrls ?? new List<string>();

            return result;
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
