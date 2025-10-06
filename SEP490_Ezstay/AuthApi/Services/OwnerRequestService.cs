using AuthApi.DTO.Request;
using AuthApi.Enums;
using AuthApi.Models;
using AuthApi.Repositories.Interfaces;
using AuthApi.Services.Interfaces;
using AutoMapper;

namespace AuthApi.Services
{
    public class OwnerRequestService : IOwnerRequestService
    {
        private readonly IAccountRepository _accountRepo;
        private readonly IOwnerRequestRepository _ownerRequestRepo;

        public OwnerRequestService(
            IAccountRepository accountRepo,
            IOwnerRequestRepository ownerRequestRepo)
        {
            _accountRepo = accountRepo;
            _ownerRequestRepo = ownerRequestRepo;
        }

        // Submit owner request
        public async Task<string> SubmitRequestAsync(string email, SubmitOwnerRequestDto dto)
        {
            // Lấy account theo email (truyền từ request)
            var account = await _accountRepo.GetByEmailAsync(email);
            if (account == null)
                return "Không tìm thấy account";

            var request = new OwnerRegistrationRequest
            {
                AccountId = account.Id,
                Reason = dto.Reason
            };

            await _ownerRequestRepo.CreateAsync(request);
            return "Đơn đăng ký đã được gửi.";
        }

        // Approve request
        public async Task<string> ApproveRequestAsync(Guid requestId)
        {
            var request = await _ownerRequestRepo.GetByIdAsync(requestId);
            if (request == null || request.Status != RequestStatusEnum.Pending)
                return "Không tìm thấy đơn hợp lệ.";

            request.Status = RequestStatusEnum.Approved;
            await _ownerRequestRepo.UpdateAsync(request);

            var account = await _accountRepo.GetByIdAsync(request.AccountId);
            if (account == null)
                return "Không tìm thấy account của đơn.";

            account.Role = RoleEnum.Owner;
            await _accountRepo.UpdateAsync(account);

            return "Đã duyệt đơn và cập nhật tài khoản thành Owner.";
        }
    }
}
