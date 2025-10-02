namespace UtilityBillAPI.Enum
{
    public enum UtilityBillStatus
    {
        Unpaid = 0, // Chưa thanh toán
        Paid = 1, // Đã thanh toán
        Cancelled = 2, // Đã hủy
        Overdue = 3, // Quá hạn
    }
}
