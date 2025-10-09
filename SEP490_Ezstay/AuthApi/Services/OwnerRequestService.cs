using AuthApi.DTO.Request;
using AuthApi.DTO.Response;
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
        private readonly IMapper _mapper;

        public OwnerRequestService(
            IAccountRepository accountRepo,
            IOwnerRequestRepository ownerRequestRepo,
            IMapper mapper)
        {
            _accountRepo = accountRepo;
            _ownerRequestRepo = ownerRequestRepo;
            _mapper = mapper;
        }

        // Submit owner request
        public async Task<OwnerRequestResponseDto?> SubmitRequestAsync(SubmitOwnerRequestDto dto)
        {
            // Tạo entity từ DTO
            var entity = _mapper.Map<OwnerRegistrationRequest>(dto);

            // Tạo Id mới nếu chưa có
            if (entity.Id == Guid.Empty)
                entity.Id = Guid.NewGuid();

            // Set thời gian submit
            entity.SubmittedAt = DateTime.UtcNow;

            // Trạng thái mặc định
            entity.Status = Enums.RequestStatusEnum.Pending;

            try
            {
                // Lưu vào MongoDB
                await _ownerRequestRepo.CreateAsync(entity);

                // Map entity thành DTO trả về
                var responseDto = _mapper.Map<OwnerRequestResponseDto>(entity);
                return responseDto;
            }
            catch (Exception ex)
            {
                // Log lỗi nếu muốn
                Console.WriteLine($"Submit owner request failed: {ex.Message}");
                return null;
            }
        }



        public async Task<string> ApproveRequestAsync(Guid requestId)
        {
            var request = await _ownerRequestRepo.GetByIdAsync(requestId);
            if (request == null || request.Status != RequestStatusEnum.Pending)
                return "Không tìm thấy đơn hợp lệ.";

            request.Status = RequestStatusEnum.Approved;
            await _ownerRequestRepo.UpdateAsync(request);

            var account = await _accountRepo.GetByIdAsync(request.AccountId);
            if (account != null)
            {
                account.Role = RoleEnum.Owner;
                await _accountRepo.UpdateAsync(account);
            }

            return "Đã duyệt đơn và cập nhật tài khoản thành Owner.";
        }
    }
}
