using RentalPostsAPI.DTO.Request;


namespace RentalPostsAPI.Service.Interface
{
    public interface IRentalPostService
    {
        public IQueryable<RentalpostDTO> GetAllAsQueryable();
        Task<RentalpostDTO> CreateAsync(CreateRentalPostDTO dto);
        Task<IEnumerable<RentalpostDTO>> GetAllAsync();
        Task<RentalpostDTO?> GetByIdAsync(Guid id);
        Task<RentalpostDTO?> UpdateAsync(Guid id, UpdateRentalPostDTO dto);
        Task<bool> DeleteAsync(Guid id, Guid deletedBy);
        Task<IEnumerable<RentalpostDTO>> GetByRoomIdAsync(Guid roomId);
    }
}
