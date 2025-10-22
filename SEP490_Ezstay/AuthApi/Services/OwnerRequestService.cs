using AuthApi.DTO.Request;
using AuthApi.DTO.Response;

using AuthApi.Models;
using AuthApi.Repositories.Interfaces;
using AuthApi.Services.Interfaces;
using AutoMapper;
using Shared.Enums;
using System.Security.Claims;

namespace AuthApi.Services
{
    public class OwnerRequestService : IOwnerRequestService
    {
        private readonly IAccountRepository _accountRepo;
        private readonly IOwnerRequestRepository _ownerRequestRepo;
        private readonly IMapper _mapper;

       private readonly HttpClient _httpClient;


    public OwnerRequestService(
        IAccountRepository accountRepo,
        IOwnerRequestRepository ownerRequestRepo,
        IMapper mapper,
        IHttpClientFactory httpFactory)
    {
        _accountRepo = accountRepo;
        _ownerRequestRepo = ownerRequestRepo;
        _mapper = mapper;
        _httpClient = httpFactory.CreateClient();
    }

        public async Task<OwnerRequestResponseDto?> SubmitRequestAsync(SubmitOwnerRequestDto dto, Guid accountId)
        {
            var entity = _mapper.Map<OwnerRegistrationRequest>(dto);

            entity.Id = Guid.NewGuid();
            entity.AccountId = accountId;           // Lấy từ token
            entity.SubmittedAt = DateTime.UtcNow;
            entity.Status = RequestStatusEnum.Pending;

            try
            {
                await _ownerRequestRepo.CreateAsync(entity);
                return _mapper.Map<OwnerRequestResponseDto>(entity);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Submit owner request failed: {ex.Message}");
                return null;
            }
        }

        public async Task<OwnerRequestResponseDto?> ApproveRequestAsync(Guid requestId, Guid staffId)
        {
            // Tìm đơn theo Id
            var request = await _ownerRequestRepo.GetByIdAsync(requestId);
            if (request == null || request.Status != RequestStatusEnum.Pending)
                return null;

            // Cập nhật thông tin duyệt
            request.Status = RequestStatusEnum.Approved;
            request.ReviewedByStaffId = staffId;
            request.ReviewedAt = DateTime.UtcNow;
            request.RejectionReason = null; // Đảm bảo không bị dính lý do từ chối cũ

            // Cập nhật đơn
            await _ownerRequestRepo.UpdateAsync(request);

            // Gán quyền owner cho account
            var account = await _accountRepo.GetByIdAsync(request.AccountId);
            if (account != null)
            {
                account.Role = Shared.Enums.RoleEnum.Owner;
                await _accountRepo.UpdateAsync(account);
            }

            // Trả về response DTO
            return _mapper.Map<OwnerRequestResponseDto>(request);
        }

        public async Task<OwnerRequestResponseDto?> RejectRequestAsync(Guid requestId, Guid staffId, string rejectionReason)
        {
            var request = await _ownerRequestRepo.GetByIdAsync(requestId);
            if (request == null || request.Status != RequestStatusEnum.Pending)
                return null;

            request.Status = RequestStatusEnum.Rejected;
            request.RejectionReason = rejectionReason;
            request.ReviewedByStaffId = staffId;
            request.ReviewedAt = DateTime.UtcNow;

            await _ownerRequestRepo.UpdateAsync(request);

            return _mapper.Map<OwnerRequestResponseDto>(request);
        }

        public async Task<List<OwnerRequestResponseDto>> GetAllRequestsAsync()
        {
            var allRequests = await _ownerRequestRepo.GetAllAsync();

            // ✅ Lọc chỉ lấy những đơn có trạng thái Pending
            var pendingRequests = allRequests
                .Where(r => r.Status == RequestStatusEnum.Pending)
                .ToList();

            return _mapper.Map<List<OwnerRequestResponseDto>>(pendingRequests);
        }

    }
}
