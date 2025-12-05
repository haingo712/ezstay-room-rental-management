using AutoMapper;
using AutoMapper.QueryableExtensions;
using RentalPostsAPI.DTO.Request;
using RentalPostsAPI.DTO.Response;
using RentalPostsAPI.Enum;
using RentalPostsAPI.Models;
using RentalPostsAPI.Repository.Interface;
using RentalPostsAPI.Service.Interface;
using System.Security.Claims;

namespace RentalPostsAPI.Service
{
    public class RentalPostService : IRentalPostService
    {
        private readonly IRentalPostRepository _repo;
        private readonly IMapper _mapper;
        private readonly ExternalService _externalService;
        private readonly ITokenService _tokenService;

        public RentalPostService(
            IRentalPostRepository repo,
            IMapper mapper,
            ExternalService externalService,
            ITokenService tokenService)
        {
            _repo = repo;
            _mapper = mapper;
            _externalService = externalService;
            _tokenService = tokenService;
        }

        public IQueryable<RentalpostDTO> GetAllAsQueryable()
        {
            return _repo.GetAllAsQueryable()
                .ProjectTo<RentalpostDTO>(_mapper.ConfigurationProvider);
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

         
            // List<RoomDto>? selectedRooms = new();
            // if (dto.RoomId != null && dto.RoomId.Any())
            // {
            //     var allRooms = await _externalService.GetRoomsByBoardingHouseIdAsync(dto.BoardingHouseId);
            //     selectedRooms = allRooms.Where(r => dto.RoomId.Contains(r.Id)).ToList();
            //
            //     if (selectedRooms.Count != dto.RoomId.Count)
            //         return ApiResponse<RentalpostDTO>.Fail("Một số phòng không thuộc trọ đã chọn");
            // }

         
            List<string>? imageUrls = null;
            if (dto.Images != null && dto.Images.Any())
            {
                imageUrls = await _externalService.UploadImagesAsync(dto.Images);
            }

       
            var entity = _mapper.Map<RentalPosts>(dto);
            entity.Id = Guid.NewGuid();
            entity.AuthorId = ownerId;
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.IsActive = true;
            entity.IsApproved = PostStatus.Pending;
            entity.ImageUrls = imageUrls ?? new List<string>();

            await _repo.CreateAsync(entity);

           // var dtoResult = await MapSingleRentalPostAsync(entity);

           var dtoResult = _mapper.Map<RentalpostDTO>(entity);
            return ApiResponse<RentalpostDTO>.Success(
                dtoResult,
                "Tạo bài đăng thành công, vui lòng chờ nhân viên duyệt"
            );
        }
        
        public async Task<IEnumerable<RentalpostDTO>> GetAllForUserAsync()
        {
            var entities = (await _repo.GetAllAsync()).ToList();
            // return await MapRentalPostsAsync(entities);
            return  _mapper.Map<IEnumerable<RentalpostDTO>>(entities);
        }

     
        public async Task<IEnumerable<RentalpostDTO>> GetAllForOwnerAsync(ClaimsPrincipal user)
        {
            var ownerId = _tokenService.GetUserIdFromClaims(user);
            var entities = (await _repo.GetAllByOwnerIdAsync(ownerId)).ToList();
            return  _mapper.Map<IEnumerable<RentalpostDTO>>(entities);
            // return await MapRentalPostsAsync(entities);
        }

       
        public async Task<RentalpostDTO> GetByIdAsync(Guid id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return null;
            
            var response = _mapper.Map<RentalpostDTO>(entity);
            var room = await _externalService.GetRoomByIdAsync(entity.RoomId);
            response.Room = room;
            response.BoardingHouse = await _externalService.GetBoardingHouseByIdAsync(entity.BoardingHouseId);
           // response.RoomName = room.RoomName;
            var account = await _externalService.GetAccountByIdAsync(entity.AuthorId);
            response.AuthorId = account.Id;
           response.AuthorName = account.FullName;
            response.ContactPhone = account.Phone;
            return response;
            
            // return await MapSingleRentalPostAsync(entity);
        }


        public async Task<RentalpostDTO> UpdateAsync(Guid id, UpdateRentalPostDTO dto)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return null;

            _mapper.Map(dto, entity);
            entity.UpdatedAt = DateTime.UtcNow;

            var updated = await _repo.UpdateAsync(entity);
            if (updated == null) return null;

            return  _mapper.Map<RentalpostDTO>(updated);
        }


        public async Task<bool> DeleteAsync(Guid id, Guid deletedBy)
        {
            return await _repo.DeleteAsync(id, deletedBy);
        }


        public async Task<IEnumerable<RentalpostDTO>> GetPendingPostsAsync()
        {
            var entities = (await _repo.GetPendingAsync()).ToList();
            // return await MapRentalPostsAsync(entities);
            return _mapper.Map<IEnumerable<RentalpostDTO>>(entities);
        }

        public async Task<bool> ApprovePostAsync(Guid postId, Guid staffId)
        {
            var entity = await _repo.GetByIdAsync(postId);
            if (entity == null || entity.IsApproved != PostStatus.Pending) return false;

            entity.IsApproved = PostStatus.Approved;
            entity.ApprovedByStaff = staffId;
            entity.ApprovedAt = DateTime.UtcNow;

            return await _repo.UpdateAsync(entity) != null;
        }

        public async Task<bool> RejectPostAsync(Guid postId, Guid staffId)
        {
            var entity = await _repo.GetByIdAsync(postId);
            if (entity == null || entity.IsApproved != PostStatus.Pending) return false;

            entity.IsApproved = PostStatus.Rejected;
            entity.ApprovedByStaff = staffId;
            entity.ApprovedAt = DateTime.UtcNow;

            return await _repo.UpdateAsync(entity) != null;
        }

        public async Task<Guid?> GetPostIdByRoomIdAsync(Guid roomId)
        {
            return await _repo.GetPostIdByRoomIdAsync(roomId);
        }

      
        // private async Task<RentalpostDTO> MapSingleRentalPostAsync(RentalPosts entity)
        // {
        //     var list = await MapRentalPostsAsync(new List<RentalPosts> { entity });
        //     return list.First();
      //  }

        // private async Task<IEnumerable<RentalpostDTO>> MapRentalPostsAsync(List<RentalPosts> entities)
        // {
        //     var dtos = _mapper.Map<List<RentalpostDTO>>(entities);
        //
        //     // Collect unique IDs
        //     var authorIds = entities.Select(e => e.AuthorId).Distinct().ToList();
        //     var roomIds = entities.Where(e => e.RoomId != null)
        //                           .ToList();
        //    
        //     var authorTaskDict = authorIds.ToDictionary(
        //         id => id,
        //         id => _externalService.GetAccountByIdAsync(id)
        //     );
        //
        //     var roomTaskList = roomIds.Select(id => _externalService.GetRoomByIdAsync(id)).ToList();
        //
        //     await Task.WhenAll(authorTaskDict.Values);
        //     var roomResults = await Task.WhenAll(roomTaskList);
        //
        //     var roomDict = roomResults
        //         .Where(r => r != null)
        //         .ToDictionary(r => r!.Id, r => r!);
        //
        //   
        //     var houseIds = roomDict.Values.Select(r => r.HouseId).Distinct().ToList();
        //     var houseTaskList = houseIds.Select(id => _externalService.GetBoardingHouseByIdAsync(id));
        //     var houseResults = await Task.WhenAll(houseTaskList);
        //
        //     var houseDict = houseResults
        //         .Where(h => h != null)
        //         .ToDictionary(h => h!.Id, h => h!);
        //
        //   
        //     for (int i = 0; i < entities.Count; i++)
        //     {
        //         var e = entities[i];
        //         var dto = dtos[i];
        //
        //         var author = await authorTaskDict[e.AuthorId];
        //         dto.AuthorName = author?.FullName ?? "Unknown";
        //         dto.ContactPhone = author?.Phone ?? "";
        //
        //         if (e.RoomId != null ))
        //         {
        //             var rooms = e.RoomId
        //                 .Where(id => roomDict.ContainsKey(id))
        //                 .Select(id => roomDict[id])
        //                 .ToList();
        //
        //             dto.RoomName = string.Join(", ", rooms.Select(r => r.RoomName));
        //
        //             var firstRoom = rooms.FirstOrDefault();
        //             if (firstRoom != null && houseDict.ContainsKey(firstRoom.HouseId))
        //             {
        //                 dto.HouseName = houseDict[firstRoom.HouseId].HouseName;
        //             }
        //
        //             // Reviews
        //             var reviews = await _externalService.GetReviewsByRoomIdsAsync(e.RoomId);
        //             dto.Reviews = reviews;
        //         }
        //
        //         dto.ImageUrls = e.ImageUrls ?? new List<string>();
        //     }
        //
        //     return dtos;
        // }
    }
}
