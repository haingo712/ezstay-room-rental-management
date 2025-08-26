using RentalRequestAPI.Model;

namespace RentalRequestAPI.Repository.Interface;

public interface IRentalRequestRepository
{
    IQueryable<RentalRequest> GetAllOdata();
    Task<IEnumerable<RentalRequest>> GetAll();
    Task<IEnumerable<RentalRequest>> GetAllByStaffId(Guid staffId);
    Task<RentalRequest?> GetByIdAsync(Guid id);
    Task AddAsync(RentalRequest rentalRequest);
    Task UpdateAsync(RentalRequest rentalRequest);
    Task DeleteAsync(RentalRequest rentalRequest);
  //  Task<bool> AmenityNameExistsAsync(string roomName);


}