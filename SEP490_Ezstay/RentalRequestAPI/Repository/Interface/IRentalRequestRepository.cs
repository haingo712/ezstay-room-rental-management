using RentalRequestAPI.Model;

namespace RentalRequestAPI.Repository.Interface;

public interface IRentalRequestRepository
{
    // IQueryable<RentalRequest> GetAllOdata();
    // Task<IEnumerable<RentalRequest>> GetAll();
    IQueryable<RentalRequest> GetAllByUserIdOdata(Guid userId);
    IQueryable<RentalRequest> GetAllByOwnerIdOdata(Guid ownerId);
    Task<IEnumerable<RentalRequest>> GetAllByUserId(Guid userId);
    Task<IEnumerable<RentalRequest>> GetAllByOwnerId(Guid ownerId);
    Task<RentalRequest?> GetByIdAsync(Guid id);
    Task AddAsync(RentalRequest rentalRequest);
    Task UpdateAsync(RentalRequest rentalRequest);
  //  Task DeleteAsync(RentalRequest rentalRequest);
  //  Task<bool> AmenityNameExistsAsync(string roomName);


}