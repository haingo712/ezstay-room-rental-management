using AuthApi.DTO.Request;
using AuthApi.DTO.Response;
using AuthApi.Enums;
using AuthApi.Models;
using AuthApi.Repositories.Interfaces;
using AuthApi.Services.Interfaces;
using AutoMapper;
using System.Security.Claims;

namespace AuthApi.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _repo;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountService(IAccountRepository repo, IMapper mapper, IHttpContextAccessor httpContextAccessor)
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

            // Staff không được update tài khoản Admin
            if (currentRole == RoleEnum.Staff && acc.Role == RoleEnum.Admin)
                return null;

            // Chỉ Admin mới được đổi mật khẩu của tài khoản Staff
            if (acc.Role == RoleEnum.Staff && currentRole != RoleEnum.Admin)
            {
                // Không thay đổi mật khẩu nếu không phải Admin
                request.Password = null;
            }

            acc.FullName = request.FullName;
            acc.Email = request.Email;
            acc.Phone = request.Phone;

            // Hash lại mật khẩu nếu có
            if (!string.IsNullOrWhiteSpace(request.Password))
            {
                acc.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
            }

            // Rule Role khi update
            acc.Role = request.Role switch
            {
                RoleEnum.Admin => RoleEnum.Staff, // Staff không được set Admin
                RoleEnum.Staff => RoleEnum.Owner,
                _ => RoleEnum.User
            };

            var updated = await _repo.UpdateAsync(acc);
            return updated == null ? null : _mapper.Map<AccountResponse>(updated);
        }


        public async Task<AccountResponse> CreateAsync(AccountRequest request)
        {
            var account = _mapper.Map<Account>(request);

            // Rule Role khi create
            account.Role = request.Role switch
            {
                RoleEnum.Admin => RoleEnum.Staff,
                RoleEnum.Staff => RoleEnum.Owner,
                _ => RoleEnum.User
            };

            account.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
            account.IsVerified = false;
            account.IsBanned = false;

            await _repo.CreateAsync(account);
            return _mapper.Map<AccountResponse>(account);
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


        public async Task<ChangePasswordResponse> ChangePasswordAsync(ChangePasswordRequest request)
        {
            var acc = await _repo.GetByEmailAsync(request.Email); // <-- dùng Email
            if (acc == null)
            {
                return new ChangePasswordResponse
                {
                    Success = false,
                    Message = "Tài khoản không tồn tại"
                };
            }

            // So sánh mật khẩu cũ
            var isMatch = BCrypt.Net.BCrypt.Verify(request.OldPassword, acc.Password);
            if (!isMatch)
            {
                return new ChangePasswordResponse
                {
                    Success = false,
                    Message = "Mật khẩu cũ không đúng"
                };
            }

            // Hash và lưu mật khẩu mới
            acc.Password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            var updated = await _repo.UpdateAsync(acc);

            if (updated == null)
            {
                return new ChangePasswordResponse
                {
                    Success = false,
                    Message = "Đổi mật khẩu thất bại"
                };
            }

            return new ChangePasswordResponse
            {
                Success = true,
                Message = "Đổi mật khẩu thành công"
            };
        }

    }

}
