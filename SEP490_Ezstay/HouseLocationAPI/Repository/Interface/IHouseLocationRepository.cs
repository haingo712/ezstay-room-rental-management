using HouseLocationAPI.Models;

namespace HouseLocationAPI.Repository.Interface
{
    public interface IHouseLocationRepository
    {        
        IQueryable<HouseLocation> GetAll();
        IQueryable<HouseLocation> GetHouseLocationsByHouseId(Guid houseId);
        Task<HouseLocation?> GetByIdAsync(Guid id);        
        Task AddAsync(HouseLocation hloc);
        Task UpdateAsync(HouseLocation hloc);
        Task DeleteAsync(HouseLocation hloc);
        Task<bool> LocationExistsWithHouseIdAsync(Guid houseId, string address);
    }
}
