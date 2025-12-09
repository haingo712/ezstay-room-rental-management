namespace UtilityBillAPI.DTO;

/// <summary>
/// System-wide financial statistics for Admin/Staff dashboard
/// Aggregates data from all owners' UtilityBills
/// </summary>
public class SystemFinancialStatisticsResponse
{
    // Platform Overview
    public decimal TotalSystemRevenue { get; set; }         // Total from all Paid bills
    public decimal TotalPendingRevenue { get; set; }        // Total from all Unpaid bills
    public decimal MonthlyRevenue { get; set; }             // Revenue this month
    public decimal PlatformCommission { get; set; }         // 5% of total revenue
    
    // Bill Counts
    public int TotalBills { get; set; }
    public int PaidBills { get; set; }
    public int UnpaidBills { get; set; }
    public int CancelledBills { get; set; }
    
    // Owner Stats
    public int TotalOwners { get; set; }                    // Unique owners with bills
    public int ActiveOwners { get; set; }                   // Owners with unpaid bills
    
    // Monthly Revenue Breakdown (last 12 months)
    public List<SystemMonthlyRevenueItem> RevenueByMonth { get; set; } = new();
    
    // Top Owners by Revenue
    public List<OwnerRevenueItem> TopOwners { get; set; } = new();
    
    // Recent System Payments
    public List<SystemPaymentItem> RecentPayments { get; set; } = new();
}

/// <summary>
/// Monthly revenue data for system charts
/// </summary>
public class SystemMonthlyRevenueItem
{
    public int Month { get; set; }
    public int Year { get; set; }
    public string MonthName { get; set; } = string.Empty;
    public decimal TotalRevenue { get; set; }
    public decimal PlatformCommission { get; set; }         // 5% commission
    public int BillCount { get; set; }
    public int OwnerCount { get; set; }                     // Unique owners this month
}

/// <summary>
/// Owner revenue ranking
/// </summary>
public class OwnerRevenueItem
{
    public Guid OwnerId { get; set; }
    public string OwnerName { get; set; } = string.Empty;
    public decimal TotalRevenue { get; set; }
    public decimal PendingAmount { get; set; }
    public int TotalBills { get; set; }
    public int PaidBills { get; set; }
    public int RoomCount { get; set; }
}

/// <summary>
/// Recent system payment item
/// </summary>
public class SystemPaymentItem
{
    public Guid BillId { get; set; }
    public Guid OwnerId { get; set; }
    public string OwnerName { get; set; } = string.Empty;
    public string RoomName { get; set; } = string.Empty;
    public string HouseName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public decimal PlatformCommission { get; set; }
    public DateTime PaidDate { get; set; }
    public string BillType { get; set; } = string.Empty;
}
