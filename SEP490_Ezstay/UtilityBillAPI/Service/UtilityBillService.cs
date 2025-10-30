using AutoMapper;
using AutoMapper.QueryableExtensions;
using UtilityBillAPI.DTO;
using UtilityBillAPI.Models;
using UtilityBillAPI.Repository.Interface;
using UtilityBillAPI.Service.Interface;
using Shared.DTOs; 
using Shared.Enums; 

namespace UtilityBillAPI.Service
{
    public class UtilityBillService : IUtilityBillService
    {
        private readonly IUtilityBillRepository _utilityBillRepo;        
        private readonly IUtilityReadingClientService _utilityReadingClientService;
        private readonly IContractClientService _contractClientService;
        private readonly IMapper _mapper;        

        public UtilityBillService(IUtilityBillRepository utilityBillRepo, IMapper mapper, 
            IContractClientService contractClientService, IUtilityReadingClientService utilityReadingClientService)
        {
            _utilityBillRepo = utilityBillRepo;
            _mapper = mapper;
            _contractClientService = contractClientService;            
            _utilityReadingClientService = utilityReadingClientService;
        }

        public IQueryable<UtilityBillDTO> GetAll()
        {
            var bills = _utilityBillRepo.GetAll();
            return bills.ProjectTo<UtilityBillDTO>(_mapper.ConfigurationProvider);
        }

        public IQueryable<UtilityBillDTO> GetAllByOwnerId(Guid ownerId)
        {
            var bills = _utilityBillRepo.GetAllByOwner(ownerId).OrderByDescending(b => b.PeriodEnd); 

            return bills.ProjectTo<UtilityBillDTO>(_mapper.ConfigurationProvider);
        }

        public IQueryable<UtilityBillDTO> GetAllByTenantId(Guid tenantId)
        {
            var bills = _utilityBillRepo.GetAllByTenant(tenantId).Where(b => b.Status != UtilityBillStatus.Cancelled);          
            return bills.ProjectTo<UtilityBillDTO>(_mapper.ConfigurationProvider);
        }       

        public async Task<UtilityBillDTO?> GetByIdAsync(Guid id)
        {
            var bill = await _utilityBillRepo.GetByIdAsync(id);
            return bill == null ? throw new KeyNotFoundException("Bill not found!") : _mapper.Map<UtilityBillDTO>(bill);
        }

        public async Task<ApiResponse<UtilityBillDTO>> GenerateUtilityBillAsync(Guid contractId)
        {
            // Check if spam bill
            //var recentBill = _utilityBillRepo.GetAll()
            //    .Where(b => b.ContractId == contractId && b.CreatedAt >= DateTime.UtcNow.AddMinutes(-1))
            //    .OrderByDescending(b => b.CreatedAt)
            //    .FirstOrDefault();

            //if (recentBill != null)
            //{
            //    return ApiResponse<UtilityBillDTO>.Fail("Vui lòng chờ ít phút trước khi tạo hoá đơn mới.");
            //}

            DateTime start, end;
            var lastBill = _utilityBillRepo.GetAllByContract(contractId)
                   .Where(b => b.Status != UtilityBillStatus.Cancelled)
                   .OrderByDescending(b => b.PeriodEnd)
                   .FirstOrDefault();
            if (lastBill == null)
            {
                var now = DateTime.UtcNow;
                start = new DateTime(now.Year, now.Month, 1);
                end = start.AddMonths(1).AddDays(-1);
            }
            else
            {
                start = lastBill.PeriodEnd.AddDays(1);
                end = start.AddMonths(1).AddDays(-1);
            }

            var contract = await _contractClientService.GetContractAsync(contractId);

            if (contract.ContractStatus != ContractStatus.Active)
            {
                string message = contract.ContractStatus switch
                {
                    ContractStatus.Pending => "Hợp đồng chưa được duyệt. Không thể tạo hoá đơn.",
                    ContractStatus.Cancelled => "Hợp đồng đã bị huỷ. Không thể tạo hoá đơn.",
                    ContractStatus.Expired => "Hợp đồng đã hết hạn. Không thể tạo hoá đơn.",
                    ContractStatus.Evicted => "Hợp đồng đã bị chấm dứt. Không thể tạo hoá đơn.",
                    _ => "Hợp đồng không hợp lệ để tạo hoá đơn."
                };

                return ApiResponse<UtilityBillDTO>.Fail(message);
            }
            
            var readings = await _utilityReadingClientService.GetUtilityReadingsAsync(contract.RoomId, start, end);

            var electric = readings.FirstOrDefault(r => r.Type == UtilityType.Electric);
            var water = readings.FirstOrDefault(r => r.Type == UtilityType.Water);

            if (electric == null || water == null)
            {                
                if (electric == null && water == null)
                    return ApiResponse<UtilityBillDTO>.Fail("Cả điện và nước chưa được cập nhật trong kỳ này.");
                if (electric == null)
                    return ApiResponse<UtilityBillDTO>.Fail("Thiếu chỉ số điện trong kỳ này.");
                return ApiResponse<UtilityBillDTO>.Fail("Thiếu chỉ số nước trong kỳ này.");
            }            

            var details = readings.Select(r => new UtilityBillDetailDTO
            {
                UtilityReadingId = r.Id,
                Type = r.Type.ToString(),
                UnitPrice = r.Price,
                Consumption = r.Consumption, 
                Total = r.Price * r.Consumption
            }).ToList();

            var totalAmount = contract.RoomPrice + details.Sum(r => r.Total);

            var bill = new UtilityBillDTO
            {
                Id = Guid.NewGuid(),
                OwnerId = contract.OwnerId,
                TenantId = contract.IdentityProfiles.FirstOrDefault(s => s.IsSigner == true)?.TenantId ?? Guid.Empty,
                RoomId = contract.RoomId,
                ContractId = contract.Id,
                RoomPrice = contract.RoomPrice,
                TotalAmount = totalAmount,
                CreatedAt = DateTime.UtcNow,
                PeriodStart = start,
                PeriodEnd = end,
                Status = UtilityBillStatus.Unpaid,
                Note = null,
                Details = details
            };

            await _utilityBillRepo.CreateAsync(_mapper.Map<UtilityBill>(bill));
            return ApiResponse<UtilityBillDTO>.Success(bill, "Tạo hoá đơn thành công");          
        } 

        public async Task<ApiResponse<bool>> MarkAsPaidAsync(Guid billId)
        {
            var bill = await _utilityBillRepo.GetByIdAsync(billId);
            if (bill.Status == UtilityBillStatus.Cancelled)
            {
                return ApiResponse<bool>.Fail("Không thể thanh toán hoá đơn đã huỷ");
            }

            await _utilityBillRepo.MarkAsPaidAsync(billId);
            return ApiResponse<bool>.Success(true, "Đã thanh toán hoá đơn thành công");
        }

        public async Task<ApiResponse<bool>> CancelAsync(Guid billId, string? reason)
        {
            var bill = await _utilityBillRepo.GetByIdAsync(billId);
            if (bill.Status == UtilityBillStatus.Paid)
            {
                return ApiResponse<bool>.Fail("Không thể huỷ hoá đơn đã thanh toán");
            }

            await _utilityBillRepo.CancelAsync(billId, reason);
            return ApiResponse<bool>.Success(true, "Đã huỷ hoá đơn thành công");
        }        

    }


}
