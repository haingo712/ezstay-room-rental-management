namespace Shared.Enums;

public enum ContractStatus
{
    Active = 1,
    Evicted = 4, // Bị đuổi, vi phạm hợp đồng
    Cancelled = 2,
    Expired = 3, //hợp đồng đã hết hạn
    Pending = 0 //hợp đồng đang chờ duyệt
}