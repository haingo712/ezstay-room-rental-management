namespace UtilityBillAPI.DTO;

/// <summary>
/// Financial statistics response for Owner dashboard
/// Aggregates data from UtilityBills to show revenue summary
/// </summary>
public class OwnerFinancialStatisticsResponse
{
    // Revenue Summary
    public decimal TotalRevenue { get; set; }           // Total revenue from all Paid bills
    public decimal MonthlyRevenue { get; set; }         // Revenue this month
    public decimal PendingRevenue { get; set; }         // Total from Unpaid bills
    
    // Bill Counts
    public int TotalBills { get; set; }                 // All bills count
    public int PaidBills { get; set; }                  // Paid bills count
    public int UnpaidBills { get; set; }                // Unpaid bills count
    public int CancelledBills { get; set; }             // Cancelled bills count
    
    // Monthly Revenue Breakdown (last 12 months)
    public List<MonthlyRevenueItem> RevenueByMonth { get; set; } = new();
    
    // Revenue by Room
    public List<RoomRevenueItem> RevenueByRoom { get; set; } = new();
    
    // Recent Paid Bills
    public List<RecentPaymentItem> RecentPayments { get; set; } = new();
}

/// <summary>
/// Monthly revenue data for charts
/// </summary>
public class MonthlyRevenueItem
{
    public int Month { get; set; }
    public int Year { get; set; }
    public string MonthName { get; set; } = string.Empty;  // "Jan", "Feb", etc.
    public decimal Revenue { get; set; }                    // Total revenue
    public int BillCount { get; set; }                      // Number of bills
}

/// <summary>
/// Revenue breakdown by room
/// </summary>
public class RoomRevenueItem
{
    public string RoomName { get; set; } = string.Empty;
    public string HouseName { get; set; } = string.Empty;
    public decimal TotalRevenue { get; set; }
    public decimal PendingAmount { get; set; }
    public int PaidBillCount { get; set; }
    public int UnpaidBillCount { get; set; }
}

/// <summary>
/// Recent payment item for display
/// </summary>
public class RecentPaymentItem
{
    public Guid BillId { get; set; }
    public string RoomName { get; set; } = string.Empty;
    public string HouseName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime PaidDate { get; set; }
    public string BillType { get; set; } = string.Empty;   // Monthly, Deposit
}
