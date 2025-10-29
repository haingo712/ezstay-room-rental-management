namespace Shared.Enums;

public enum PaymentStatus
{
    Pending,           // Chờ xử lý (vừa tạo)
    Processing,        // Đang xử lý (đang chờ check SePay cho online payment)
    PendingApproval,   // Chờ duyệt (offline payment chờ owner xác nhận)
    Approved,          // Đã duyệt (offline payment được owner chấp nhận)
    Success,           // Thành công (đã thanh toán hoàn tất)
    Failed,            // Thất bại
    Rejected,          // Bị từ chối (offline payment bị owner từ chối)
    Cancelled          // Hủy
}