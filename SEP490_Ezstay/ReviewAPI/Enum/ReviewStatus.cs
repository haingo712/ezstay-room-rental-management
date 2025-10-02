namespace ReviewAPI.Enum;

public enum ReviewStatus
{
    Pending,    // chờ duyệt (optional)
    Approved,   // hiển thị bình thường
    Reported,   // bị chủ trọ báo cáo
    Hidden,     // nhân viên đã ẩn
    Deleted     // nhân viên đã xoá
}