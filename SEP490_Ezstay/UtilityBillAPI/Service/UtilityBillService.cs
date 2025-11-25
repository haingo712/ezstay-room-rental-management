using AutoMapper;
using AutoMapper.QueryableExtensions;
using UtilityBillAPI.DTO;
using UtilityBillAPI.Models;
using UtilityBillAPI.Repository.Interface;
using UtilityBillAPI.Service.Interface;
using Shared.DTOs;
using Shared.DTOs.UtilityReadings.Responses;
using Shared.Enums;
using MongoDB.Driver;

namespace UtilityBillAPI.Service
{
    public class UtilityBillService : IUtilityBillService
    {
        private readonly IUtilityBillRepository _utilityBillRepo;
        private readonly IUtilityReadingService _utilityReadingService;
        private readonly IContractService _contractService;
        private readonly IMapper _mapper;

        public UtilityBillService(IUtilityBillRepository utilityBillRepo, IMapper mapper,
            IContractService contractService, IUtilityReadingService utilityReadingService)
        {
            _utilityBillRepo = utilityBillRepo;
            _mapper = mapper;
            _contractService = contractService;
            _utilityReadingService = utilityReadingService;
        }

        public IQueryable<UtilityBillDTO> GetAll()
        {
            var bills = _utilityBillRepo.GetAll();
            return bills.ProjectTo<UtilityBillDTO>(_mapper.ConfigurationProvider);
        }

        public IQueryable<UtilityBillDTO> GetAllByOwnerId(Guid ownerId)
        {
            var bills = _utilityBillRepo.GetAllByOwner(ownerId).OrderByDescending(b => b.CreatedAt);

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

        public async Task<ApiResponse<UtilityBillDTO>> GenerateMonthlyBillAsync(Guid contractId, Guid ownerId)
        {
            // Check if spam bill
            //var recentBill = _utilityBillRepo.GetAll()
            //    .Where(b => b.ContractId == contractId && b.CreatedAt >= DateTime.UtcNow.AddMinutes(-1))
            //    .OrderByDescending(b => b.CreatedAt)
            //    .FirstOrDefault();

            //if (recentBill != null)
            //{
            //    return ApiResponse<UtilityBillDTO>.Fail("Please wait a few minutes before creating a new bill.");
            //}          

            var contract = await _contractService.GetContractAsync(contractId);

            if (contract.ContractStatus != ContractStatus.Active)
            {
                string message = contract.ContractStatus switch
                {
                    ContractStatus.Pending => "Contract is pending approval. Cannot generate a bill.",
                    ContractStatus.Cancelled => "Contract is cancelled. Cannot generate a bill.",
                    ContractStatus.Expired => "Contract is expired. Cannot generate a bill.",
                    ContractStatus.Evicted => "Contract has been terminated. Cannot generate a bill.",
                    _ => "Contract is not valid for bill generation."
                };

                return ApiResponse<UtilityBillDTO>.Fail(message);
            }

            var readings = await _utilityReadingService.GetUtilityReadingsAsync(contract.Id);         

            // Get the latest reading
            var latest = readings.MaxBy(r => r.ReadingDate);

            if (latest == null)
                return ApiResponse<UtilityBillDTO>.Fail("No readings found.");

            // Extract billing period
            int billingMonth = latest.ReadingDate.Month;
            int billingYear = latest.ReadingDate.Year;

            // Filter readings by billing period
            var readingsForMonth = readings
                .Where(r => r.ReadingDate.Year == billingYear &&
                            r.ReadingDate.Month == billingMonth)
                .ToList();

            if (readingsForMonth.Count == 0)
                return ApiResponse<UtilityBillDTO>.Fail("No readings found for the target billing month.");


            var details = new List<UtilityBillDetailDTO>();
            foreach (var r in readings)
            {
                details.Add(new UtilityBillDetailDTO
                {
                    UtilityReadingId = r.Id,
                    Type = r.Type.ToString(),
                    Total = r.Total,
                    UtilityReading = new UtilityReadingResponse
                    {
                        Id = r.Id,
                        ContractId = r.ContractId,
                        Type = r.Type,
                        PreviousIndex = r.PreviousIndex,
                        CurrentIndex = r.CurrentIndex,
                        Consumption = r.Consumption,
                        ReadingDate = r.ReadingDate,
                        Price = r.Price,
                        Total = r.Total
                    }
                });
            }

            foreach (var s in contract.ServiceInfors)
            {
                details.Add(new UtilityBillDetailDTO
                {
                    ServiceName = s.ServiceName,
                    ServicePrice = s.Price,
                    Type = "Service",
                    Total = s.Price
                });
            }

            var totalAmount = contract.RoomPrice + details.Sum(r => r.Total);

            var bill = new UtilityBillDTO
            {
                Id = Guid.NewGuid(),
                TenantId = contract.IdentityProfiles.FirstOrDefault(s => s.IsSigner == true)?.UserId ?? Guid.Empty,
                OwnerId = ownerId,
                RoomId = contract.RoomId,
                ContractId = contract.Id,
                RoomPrice = contract.RoomPrice,
                TotalAmount = totalAmount,
                CreatedAt = DateTime.UtcNow,              
                Status = UtilityBillStatus.Unpaid,
                BillType = UtilityBillType.Monthly,
                BillingMonth = billingMonth,
                BillingYear = billingYear,
                Note = $"Monthly bill for {billingMonth}/{billingYear}",
                Details = details
            };

            await _utilityBillRepo.CreateAsync(_mapper.Map<UtilityBill>(bill));
            return ApiResponse<UtilityBillDTO>.Success(bill, "Monthly bill created successfully.");
        }

        public async Task<ApiResponse<UtilityBillDTO>> GenerateDepositBillAsync(Guid contractId, Guid ownerId)
        {
            var contract = await _contractService.GetContractAsync(contractId);
            if (contract.ContractStatus != ContractStatus.Active)
            {
                string message = contract.ContractStatus switch
                {
                    ContractStatus.Pending => "Contract is pending approval. Cannot generate a bill.",
                    ContractStatus.Cancelled => "Contract is cancelled. Cannot generate a bill.",
                    ContractStatus.Expired => "Contract is expired. Cannot generate a bill.",
                    ContractStatus.Evicted => "Contract has been terminated. Cannot generate a bill.",
                    _ => "Contract is not valid for bill generation."
                };

                return ApiResponse<UtilityBillDTO>.Fail(message);
            }

            var bill = new UtilityBillDTO
            {
                Id = Guid.NewGuid(),
                TenantId = contract.IdentityProfiles.FirstOrDefault(p => p.IsSigner)?.UserId ?? Guid.Empty,
                OwnerId = ownerId,
                RoomId = contract.RoomId,
                ContractId = contract.Id,
                RoomPrice = 0,                
                TotalAmount = contract.DepositAmount,
                CreatedAt = DateTime.UtcNow,               
                Status = UtilityBillStatus.Unpaid,
                BillType = UtilityBillType.Deposit,
                Note = "Deposit payment",
                Details = new List<UtilityBillDetailDTO>
                {
                    new UtilityBillDetailDTO
                    {
                        Type = "Deposit",
                        Total = contract.DepositAmount,
                        ServiceName = "Deposit Fee",
                        ServicePrice = contract.DepositAmount
                    }
                }
            };

            await _utilityBillRepo.CreateAsync(_mapper.Map<UtilityBill>(bill));

            return ApiResponse<UtilityBillDTO>.Success(bill, "Deposit bill created successfully.");
        }


        public async Task<ApiResponse<bool>> MarkAsPaidAsync(Guid billId)
        {
            var bill = await _utilityBillRepo.GetByIdAsync(billId);
            if (bill.Status == UtilityBillStatus.Cancelled)
            {
                return ApiResponse<bool>.Fail("Unable to pay canceled invoice.");
            }

            await _utilityBillRepo.MarkAsPaidAsync(billId);
            return ApiResponse<bool>.Success(true, "Bill paid successfully!");
        }

        public async Task<ApiResponse<bool>> CancelAsync(Guid billId, string? reason)
        {
            var bill = await _utilityBillRepo.GetByIdAsync(billId);
            if (bill.Status == UtilityBillStatus.Paid)
            {
                return ApiResponse<bool>.Fail("Cannot cancel paid invoice.");
            }

            await _utilityBillRepo.CancelAsync(billId, reason);
            return ApiResponse<bool>.Success(true, "Bill canceled successfully!");
        }

    }


}
