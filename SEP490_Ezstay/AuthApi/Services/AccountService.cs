using AuthApi.DTO.Request;
using Shared.DTOs.Auths.Responses; // 👈 thay vì dùng AuthApi.DTO.Response


using AuthApi.Models;
using AuthApi.Repositories.Interfaces;
using AuthApi.Services.Interfaces;
using AutoMapper;
using System.Security.Claims;
using Shared.DTOs;
using Shared.Enums;

namespace AuthApi.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAuthRepository _repo;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
     

        public AccountService(IAuthRepository repo, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _repo = repo;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        
       
        }

        private RoleEnum GetCurrentUserRole()
        {
            var roleClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value;

            return roleClaim switch
            {
                "Admin" => RoleEnum.Admin,
                "Staff" => RoleEnum.Staff,
                "Owner" => RoleEnum.Owner,
                _ => RoleEnum.User
            };
        }

        public async Task<AccountResponse?> GetByIdAsync(Guid id)
        {
            var acc = await _repo.GetByIdAsync(id);
            if (acc == null) return null;

            var currentRole = GetCurrentUserRole();


            return _mapper.Map<AccountResponse>(acc);
        }

        public async Task<List<AccountResponse>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();
            var currentRole = GetCurrentUserRole();

            if (currentRole == RoleEnum.Staff)
            {
                // Staff KHÔNG được thấy Admin và KHÔNG được thấy Staff
                list = list.Where(x => x.Role != RoleEnum.Admin && x.Role != RoleEnum.Staff).ToList();
            }
            else if (currentRole == RoleEnum.Admin)
            {
                // Admin chỉ được thấy Staff
                list = list.Where(x => x.Role == RoleEnum.Staff).ToList();
            }

            return _mapper.Map<List<AccountResponse>>(list);
        }

        public async Task<AccountResponse?> UpdateAsync(Guid id, AccountRequest request)
        {
            var acc = await _repo.GetByIdAsync(id);
            if (acc == null) return null;

            var currentRole = GetCurrentUserRole();

            // ❌ Không cho Staff update Admin
            if (currentRole == RoleEnum.Staff && acc.Role == RoleEnum.Admin)
                return null;

            // ✅ Check quyền sửa role
            var isAllowed = (currentRole, request.Role) switch
            {
                (RoleEnum.Admin, RoleEnum.Staff) => true,
                (RoleEnum.Staff, RoleEnum.Owner) => true,
                (RoleEnum.Staff, RoleEnum.Staff) => true,
                (RoleEnum.Staff, RoleEnum.User) => true,
                _ => false
};

            if (!isAllowed)
                throw new UnauthorizedAccessException("Bạn không có quyền cập nhật tài khoản với vai trò này.");

            acc.FullName = request.FullName;
            acc.Email = request.Email;
            acc.Phone = request.Phone;
            acc.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
            acc.Role = request.Role;
            
            var updated = await _repo.UpdateAsync(acc);
            return updated == null ? null : _mapper.Map<AccountResponse>(updated);
        }

        public async Task<ApiResponse<AccountResponse>> CreateAsync(AccountRequest request)
        {
            var creatorRole = GetCurrentUserRole();

            var isAllowed = (creatorRole, request.Role) switch
            {
                (RoleEnum.Admin, RoleEnum.Staff) => true,
                (RoleEnum.Staff, RoleEnum.Owner) => true,

                (RoleEnum.Staff, RoleEnum.User) => true,
                _ => false
            };

            if (!isAllowed)
                throw new UnauthorizedAccessException("Bạn không có quyền tạo tài khoản với vai trò này.");

            var existing = await _repo.GetByEmailAsync(request.Email);
            if (existing != null)
                return ApiResponse<AccountResponse>.Fail("Email Already Exists");

            var account = _mapper.Map<Account>(request);
            account.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
            account.IsVerified = true;
            account.IsBanned = false;
            account.CreateAt = DateTime.UtcNow;

            await _repo.CreateAsync(account);
            return ApiResponse<AccountResponse>.Success(_mapper.Map<AccountResponse>(account)); 
        }


        public async Task<bool> UpdateFullNameAsync(Guid id, string fullName)
        {
            var acc = await _repo.GetByIdAsync(id);
            if (acc == null) return false;

            acc.FullName = fullName; // ✅ chỉ cập nhật 1 field

            var updated = await _repo.UpdateAsync(acc);
            return updated != null;
        }



        public async Task VerifyAsync(string email) => await _repo.MarkAsVerified(email);
        public async Task BanAsync(Guid id) => await _repo.BanAccountAsync(id, true);
        public async Task UnbanAsync(Guid id) => await _repo.BanAccountAsync(id, false);

        public async Task<string> ChangePasswordAsync(ChangePasswordRequest dto)
        {
            var account = await _repo.GetByEmailAsync(dto.Email);
            if (account == null)
            {
                return "Tài khoản không tồn tại.";
            }

            // ✅ So sánh mật khẩu cũ bằng BCrypt
            if (!BCrypt.Net.BCrypt.Verify(dto.OldPassword, account.Password))
            {
                return "The current password is incorrect.";
            }

            if (dto.NewPassword == dto.OldPassword)
            {
                return "The new password must be different from the current password.";
            }

            // ✅ Mã hóa mật khẩu mới
            account.Password = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

            var result = await _repo.UpdateAsync(account);

            return result != null
                ? "Đổi mật khẩu thành công."
                : "Đổi mật khẩu thất bại.";
        }

        public async Task<List<AccountResponse>> GetByRoleAsync(Shared.Enums.RoleEnum role)
        {
            var list = await _repo.GetByRoleAsync(role);
            return _mapper.Map<List<AccountResponse>>(list);
        }


    }



}