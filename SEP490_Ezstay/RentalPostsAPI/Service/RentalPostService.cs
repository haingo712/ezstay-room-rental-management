using AutoMapper;
using AutoMapper.QueryableExtensions;
using RentalPostsAPI.DTO.Request;

using RentalPostsAPI.Models;
using RentalPostsAPI.Repository.Interface;
using RentalPostsAPI.Service.Interface;

namespace RentalPostsAPI.Service
{
    public class RentalPostService : IRentalPostService
    {
        private readonly IRentalPostRepository _repo;
        private readonly IMapper _mapper;

        public RentalPostService(IRentalPostRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        public IQueryable<RentalpostDTO> GetAllAsQueryable()
        {
            var entity = _repo.GetAllAsQueryable();
            return entity.ProjectTo<RentalpostDTO>(_mapper.ConfigurationProvider);
        }

        public async Task<RentalpostDTO> CreateAsync(CreateRentalPostDTO dto)
        {
            var entity = _mapper.Map<RentalPosts>(dto);
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;

            var created = await _repo.CreateAsync(entity);
            return _mapper.Map<RentalpostDTO>(created);
        }

        public async Task<IEnumerable<RentalpostDTO>> GetAllAsync()
        {
            var entities = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<RentalpostDTO>>(entities);
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

        public async Task<IEnumerable<RentalpostDTO>> GetByRoomIdAsync(Guid roomId)
        {
            var entity= await _repo.GetByRoomIdAsync(roomId);
            return   _mapper.Map<IEnumerable<RentalpostDTO>>(entity);
        }
    }
}
