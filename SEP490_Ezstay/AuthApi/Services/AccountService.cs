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

        public AccountService(
            IAccountRepository repo,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _repo = repo;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<AccountResponse> CreateAsync(AccountRequest request)
        {
            var account = _mapper.Map<Account>(request);

            // Rule xử lý Role
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

        public async Task<AccountResponse?> GetByIdAsync(Guid id)
        {
            var acc = await _repo.GetByIdAsync(id);
            return acc == null ? null : _mapper.Map<AccountResponse>(acc);
        }

        public async Task<List<AccountResponse>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();

            // Lấy role hiện tại từ Claims
            var role = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value;

            // Nếu Staff thì không được xem Admin
            if (role?.Equals(RoleEnum.Staff.ToString(), StringComparison.OrdinalIgnoreCase) == true)
            {
                list = list.Where(x => x.Role != RoleEnum.Admin).ToList();
            }

            return _mapper.Map<List<AccountResponse>>(list);
        }

        public async Task<AccountResponse?> UpdateAsync(Guid id, AccountRequest request)
        {
            var acc = await _repo.GetByIdAsync(id);
            if (acc == null) return null;

            acc.FullName = request.FullName;
            acc.Email = request.Email;
            acc.Phone = request.Phone;
            acc.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);

            // Rule Role khi update
            acc.Role = request.Role switch
            {
                RoleEnum.Admin => RoleEnum.Staff,
                RoleEnum.Staff => RoleEnum.Owner,
                _ => RoleEnum.User
            };

            var updated = await _repo.UpdateAsync(acc);
            return updated == null ? null : _mapper.Map<AccountResponse>(updated);
        }

        public async Task VerifyAsync(string email)
        {
            await _repo.MarkAsVerified(email);
        }

        public async Task BanAsync(Guid id)
        {
            await _repo.BanAccountAsync(id, true);
        }

        public async Task UnbanAsync(Guid id)
        {
            await _repo.BanAccountAsync(id, false);
        }
    }
}
