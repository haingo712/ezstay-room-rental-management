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
using Microsoft.AspNetCore.Mvc;

namespace UtilityBillAPI.Service
{
    public class UtilityBillService : IUtilityBillService
    {
        private readonly IUtilityBillRepository _utilityBillRepo;
        private readonly IUtilityReadingService _utilityReadingService;
        private readonly IContractService _contractService;
        private readonly IRoomInfoService _roomInfoService;
        private readonly IMapper _mapper;

        public UtilityBillService(IUtilityBillRepository utilityBillRepo, IMapper mapper,
            IContractService contractService, IUtilityReadingService utilityReadingService, 
            IRoomInfoService roomInforService)
        {
            _utilityBillRepo = utilityBillRepo;
            _mapper = mapper;
            _contractService = contractService;
            _utilityReadingService = utilityReadingService;
            _roomInfoService = roomInforService; 
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
            if (contract == null)
                return ApiResponse<UtilityBillDTO>.Fail("Contract not found.");

            if (contract.ContractStatus != ContractStatus.Active)
            {
                string message = contract.ContractStatus switch
                {
                    ContractStatus.Pending => "Contract is pending approval. Cannot generate a bill.",
                    ContractStatus.Cancelled => "Contract is cancelled. Cannot generate a bill.",
                    ContractStatus.Expired => "Contract is expired. Cannot generate a bill.",
                    _ => "Contract is not valid for bill generation."
                };

                return ApiResponse<UtilityBillDTO>.Fail(message);
            }

            DateTime today = DateTime.UtcNow;
            int billingMonth = today.Month;
            int billingYear = today.Year;       
            
            var existingMonthlyBill = _utilityBillRepo.GetAll()
                .FirstOrDefault(b => b.ContractId == contractId
                && b.BillType == UtilityBillType.Monthly
                && b.CreatedAt.Month == billingMonth
                && b.CreatedAt.Year == billingYear
                && b.Status != UtilityBillStatus.Cancelled);
            if (existingMonthlyBill != null)
            {
                return ApiResponse<UtilityBillDTO>.Fail(
                   $"A bill for {billingMonth}/{billingYear} already exists (Bill ID: {existingMonthlyBill.Id}). " +
                   $"If you want to generate a new bill, please cancel the existing bill first."
                );
            }

            var electric = await _utilityReadingService.GetElectricityReadingAsync(contract.Id, billingMonth, billingYear);
            var water = await _utilityReadingService.GetWaterReadingAsync(contract.Id, billingMonth, billingYear);

            if (electric == null && water == null)
                return ApiResponse<UtilityBillDTO>.Fail($"Missing both electricity and water readings for {billingMonth}/{billingYear}.");

            if (electric == null)
                return ApiResponse<UtilityBillDTO>.Fail($"Missing electricity reading for {billingMonth}/{billingYear}.");

            if (water == null)
                return ApiResponse<UtilityBillDTO>.Fail($"Missing water reading for {billingMonth}/{billingYear}.");

            var readings = new[] { electric, water };

            var details = readings
               .Select(r => new UtilityBillDetailDTO
               {
                   Type = r.Type.ToString(),
                   UnitPrice = r.Price,
                   PreviousIndex = r.PreviousIndex,
                   CurrentIndex = r.CurrentIndex,
                   Consumption = r.Consumption,
                   Total = r.Total
               }).ToList();

            if (contract.ServiceInfors != null)
            {
                details.AddRange(
                    contract.ServiceInfors.Select(s => new UtilityBillDetailDTO
                    {
                        Type = "Service",
                        ServiceName = s.ServiceName,
                        ServicePrice = s.Price,
                        Total = s.Price
                    })
                );
            }

            var roomInfo = await _roomInfoService.GetRoomInfoAsync(contract.RoomId);

            var totalAmount = contract.RoomPrice + details.Sum(r => r.Total);

            var bill = new UtilityBillDTO
            {
                Id = Guid.NewGuid(),
                TenantId = contract.IdentityProfiles.FirstOrDefault(s => s.IsSigner == true && s.UserId != ownerId)?.UserId ?? Guid.Empty,
                OwnerId = ownerId,                
                ContractId = contract.Id,
                RoomName = roomInfo.RoomName,
                HouseName = roomInfo.HouseName,
                RoomPrice = contract.RoomPrice,
                TotalAmount = totalAmount,
                CreatedAt = DateTime.UtcNow,
                Status = UtilityBillStatus.Unpaid,
                BillType = UtilityBillType.Monthly,              
                Note = $"Monthly bill for {billingMonth}/{billingYear}",
                Details = details
            };

            await _utilityBillRepo.CreateAsync(_mapper.Map<UtilityBill>(bill));
            return ApiResponse<UtilityBillDTO>.Success(bill, "Monthly bill created successfully.");
        }

        public async Task<ApiResponse<UtilityBillDTO>> GenerateDepositBillAsync(Guid contractId, Guid ownerId)
        {
            var contract = await _contractService.GetContractAsync(contractId);
            if (contract == null)
                return ApiResponse<UtilityBillDTO>.Fail("Contract not found.");
            if (contract.ContractStatus != ContractStatus.Active)
            {
                string message = contract.ContractStatus switch
                {
                    ContractStatus.Pending => "Contract is pending approval. Cannot generate a bill.",
                    ContractStatus.Cancelled => "Contract is cancelled. Cannot generate a bill.",
                    ContractStatus.Expired => "Contract is expired. Cannot generate a bill.",
                 //   ContractStatus.Evicted => "Contract has been terminated. Cannot generate a bill.",
                    _ => "Contract is not valid for bill generation."
                };

                return ApiResponse<UtilityBillDTO>.Fail(message);
            }

            var existingDepositBill = _utilityBillRepo.GetAll()
                .FirstOrDefault(b => b.ContractId == contractId
                && b.BillType == UtilityBillType.Deposit
                && b.Status != UtilityBillStatus.Cancelled);

            if (existingDepositBill != null)
            {
                return ApiResponse<UtilityBillDTO>.Fail("A deposit bill for this contract already exists and cannot be created again.");
            }

            var roomInfo = await _roomInfoService.GetRoomInfoAsync(contract.RoomId);

            // Get TenantId: Find the signer who is not the owner
            // If IdentityProfiles is null/empty, try to get from contract's other identity info
            var tenantId = contract.IdentityProfiles?
                .FirstOrDefault(s => s.IsSigner && s.UserId != ownerId)?.UserId ?? Guid.Empty;
            
            // If still empty, log warning - deposit bill will be created but tenant won't see it in their bill list
            if (tenantId == Guid.Empty)
            {
                Console.WriteLine($"⚠️ Warning: Could not find TenantId for contract {contractId}. IdentityProfiles count: {contract.IdentityProfiles?.Count ?? 0}");
            }

            var bill = new UtilityBillDTO
            {
                Id = Guid.NewGuid(),
                TenantId = tenantId,
                OwnerId = ownerId,                
                ContractId = contract.Id,
                RoomName = roomInfo.RoomName,
                HouseName = roomInfo.HouseName,                
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

        /// <summary>
        /// Get financial statistics for owner dashboard
        /// Aggregates data from all owner's bills
        /// </summary>
        public async Task<OwnerFinancialStatisticsResponse> GetFinancialStatisticsAsync(Guid ownerId, int? year = null)
        {
            var targetYear = year ?? DateTime.UtcNow.Year;
            var currentMonth = DateTime.UtcNow.Month;
            
            // Get all bills for this owner
            var allBills = _utilityBillRepo.GetAllByOwner(ownerId).ToList();
            
            // Calculate revenue summary
            var paidBills = allBills.Where(b => b.Status == UtilityBillStatus.Paid).ToList();
            var unpaidBills = allBills.Where(b => b.Status == UtilityBillStatus.Unpaid).ToList();
            var cancelledBills = allBills.Where(b => b.Status == UtilityBillStatus.Cancelled).ToList();
            
            var totalRevenue = paidBills.Sum(b => b.TotalAmount);
            var monthlyRevenue = paidBills
                .Where(b => b.CreatedAt.Year == targetYear && b.CreatedAt.Month == currentMonth)
                .Sum(b => b.TotalAmount);
            var pendingRevenue = unpaidBills.Sum(b => b.TotalAmount);
            
            // Monthly revenue breakdown (last 12 months)
            var revenueByMonth = new List<MonthlyRevenueItem>();
            var monthNames = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            
            for (int i = 11; i >= 0; i--)
            {
                var date = DateTime.UtcNow.AddMonths(-i);
                var monthBills = paidBills
                    .Where(b => b.CreatedAt.Year == date.Year && b.CreatedAt.Month == date.Month)
                    .ToList();
                
                revenueByMonth.Add(new MonthlyRevenueItem
                {
                    Month = date.Month,
                    Year = date.Year,
                    MonthName = monthNames[date.Month - 1],
                    Revenue = monthBills.Sum(b => b.TotalAmount),
                    BillCount = monthBills.Count
                });
            }
            
            // Revenue by room
            var revenueByRoom = allBills
                .GroupBy(b => new { b.RoomName, b.HouseName })
                .Select(g => new RoomRevenueItem
                {
                    RoomName = g.Key.RoomName ?? "Unknown",
                    HouseName = g.Key.HouseName ?? "Unknown",
                    TotalRevenue = g.Where(b => b.Status == UtilityBillStatus.Paid).Sum(b => b.TotalAmount),
                    PendingAmount = g.Where(b => b.Status == UtilityBillStatus.Unpaid).Sum(b => b.TotalAmount),
                    PaidBillCount = g.Count(b => b.Status == UtilityBillStatus.Paid),
                    UnpaidBillCount = g.Count(b => b.Status == UtilityBillStatus.Unpaid)
                })
                .OrderByDescending(r => r.TotalRevenue)
                .ToList();
            
            // Recent payments (last 10)
            var recentPayments = paidBills
                .OrderByDescending(b => b.UpdatedAt ?? b.CreatedAt)
                .Take(10)
                .Select(b => new RecentPaymentItem
                {
                    BillId = b.Id,
                    RoomName = b.RoomName ?? "Unknown",
                    HouseName = b.HouseName ?? "Unknown",
                    Amount = b.TotalAmount,
                    PaidDate = b.UpdatedAt ?? b.CreatedAt,
                    BillType = b.BillType.ToString()
                })
                .ToList();
            
            return new OwnerFinancialStatisticsResponse
            {
                TotalRevenue = totalRevenue,
                MonthlyRevenue = monthlyRevenue,
                PendingRevenue = pendingRevenue,
                TotalBills = allBills.Count,
                PaidBills = paidBills.Count,
                UnpaidBills = unpaidBills.Count,
                CancelledBills = cancelledBills.Count,
                RevenueByRoom = revenueByRoom,
                RecentPayments = recentPayments
            };
        }

        /// <summary>
        /// Get system-wide financial statistics for Admin/Staff dashboard
        /// Aggregates data from ALL owners' bills
        /// </summary>
        public async Task<SystemFinancialStatisticsResponse> GetSystemFinancialStatisticsAsync(int? year = null)
        {
            var targetYear = year ?? DateTime.UtcNow.Year;
            var currentMonth = DateTime.UtcNow.Month;
            
            // Get ALL bills in the system
            var allBills = _utilityBillRepo.GetAll().ToList();
            
            // Calculate system-wide statistics
            var paidBills = allBills.Where(b => b.Status == UtilityBillStatus.Paid).ToList();
            var unpaidBills = allBills.Where(b => b.Status == UtilityBillStatus.Unpaid).ToList();
            var cancelledBills = allBills.Where(b => b.Status == UtilityBillStatus.Cancelled).ToList();
            
            var totalRevenue = paidBills.Sum(b => b.TotalAmount);
            var pendingRevenue = unpaidBills.Sum(b => b.TotalAmount);
            var monthlyRevenue = paidBills
                .Where(b => b.CreatedAt.Year == targetYear && b.CreatedAt.Month == currentMonth)
                .Sum(b => b.TotalAmount);
            
            // 5% platform commission
            var platformCommission = totalRevenue * 0.05m;
            
            // Count unique owners
            var allOwnerIds = allBills.Select(b => b.OwnerId).Distinct().ToList();
            var activeOwnerIds = unpaidBills.Select(b => b.OwnerId).Distinct().ToList();
            
            // Monthly revenue breakdown (last 12 months)
            var revenueByMonth = new List<SystemMonthlyRevenueItem>();
            var monthNames = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            
            for (int i = 11; i >= 0; i--)
            {
                var date = DateTime.UtcNow.AddMonths(-i);
                var monthBills = paidBills
                    .Where(b => b.CreatedAt.Year == date.Year && b.CreatedAt.Month == date.Month)
                    .ToList();
                
                var monthRevenue = monthBills.Sum(b => b.TotalAmount);
                
                revenueByMonth.Add(new SystemMonthlyRevenueItem
                {
                    Month = date.Month,
                    Year = date.Year,
                    MonthName = monthNames[date.Month - 1],
                    TotalRevenue = monthRevenue,
                    PlatformCommission = monthRevenue * 0.05m,
                    BillCount = monthBills.Count,
                    OwnerCount = monthBills.Select(b => b.OwnerId).Distinct().Count()
                });
            }
            
            // Top owners by revenue
            var topOwners = allBills
                .GroupBy(b => b.OwnerId)
                .Select(g => new OwnerRevenueItem
                {
                    OwnerId = g.Key,
                    OwnerName = $"Owner {g.Key.ToString().Substring(0, 8)}...",
                    TotalRevenue = g.Where(b => b.Status == UtilityBillStatus.Paid).Sum(b => b.TotalAmount),
                    PendingAmount = g.Where(b => b.Status == UtilityBillStatus.Unpaid).Sum(b => b.TotalAmount),
                    TotalBills = g.Count(),
                    PaidBills = g.Count(b => b.Status == UtilityBillStatus.Paid),
                    RoomCount = g.Select(b => b.RoomName).Distinct().Count()
                })
                .OrderByDescending(o => o.TotalRevenue)
                .Take(10)
                .ToList();
            
            // Recent system payments (last 20)
            var recentPayments = paidBills
                .OrderByDescending(b => b.UpdatedAt ?? b.CreatedAt)
                .Take(20)
                .Select(b => new SystemPaymentItem
                {
                    BillId = b.Id,
                    OwnerId = b.OwnerId,
                    OwnerName = $"Owner {b.OwnerId.ToString().Substring(0, 8)}...",
                    RoomName = b.RoomName ?? "Unknown",
                    HouseName = b.HouseName ?? "Unknown",
                    Amount = b.TotalAmount,
                    PlatformCommission = b.TotalAmount * 0.05m,
                    PaidDate = b.UpdatedAt ?? b.CreatedAt,
                    BillType = b.BillType.ToString()
                })
                .ToList();
            
            return new SystemFinancialStatisticsResponse
            {
                TotalSystemRevenue = totalRevenue,
                TotalPendingRevenue = pendingRevenue,
                MonthlyRevenue = monthlyRevenue,
                PlatformCommission = platformCommission,
                TotalBills = allBills.Count,
                PaidBills = paidBills.Count,
                UnpaidBills = unpaidBills.Count,
                CancelledBills = cancelledBills.Count,
                TotalOwners = allOwnerIds.Count,
                ActiveOwners = activeOwnerIds.Count,
                RevenueByMonth = revenueByMonth,
                TopOwners = topOwners,
                RecentPayments = recentPayments
            };
        }

    }


}
