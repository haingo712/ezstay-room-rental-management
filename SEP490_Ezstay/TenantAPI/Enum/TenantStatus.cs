namespace TenantAPI.Enum;

public enum TenantStatus
{
    Active = 1,
    Evicted = 2, // Bị đuổi, vi phạm hợp đồng
    Cancelled = 3  
}