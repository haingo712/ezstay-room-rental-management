// using Microsoft.EntityFrameworkCore;
// using TenantAPI.Model;
// using TenantAPI.Repository.Interface;
//
// namespace TenantAPI.Repository;
//
// public class TenantRepository:ITenantRepository
// {
//     private readonly AppDbContext _context;
//
//     public TenantRepository(AppDbContext context)
//     {
//         _context = context;
//     }
//
//     public IQueryable<Tenant> GetAll()=> _context.Tenants.AsQueryable();
//
//     public async Task<Tenant?> GetByIdAsync(int id)
//     {
//       return await _context.Tenants.FindAsync(id);
//     }
//
//     public async Task AddAsync(Tenant tenant)
//     {
//        _context.Tenants.Add(tenant);
//        await _context.SaveChangesAsync();
//     }
//     public async Task<bool> TenantRoomIsActiveAsync(int roomId)
//         => await _context.Tenants
//             .AnyAsync(t => t.RoomId == roomId && t.IsActive == true);
//     
//     // public async Task<bool> RoomNameExistsInHouse(int houseId, string roomName)
//     // {
//     //     return await _context.Rooms
//     //         .AnyAsync(r => r.HouseId == houseId && r.RoomName.ToLower() == roomName.ToLower());
//     // }
//     public async  Task UpdateAsync(Tenant tenant)
//     {
//         _context.Tenants.Update(tenant);
//         await _context.SaveChangesAsync();
//     }
//     
// }