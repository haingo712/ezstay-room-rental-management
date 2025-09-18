namespace TenantAPI.Enum;

public enum TenantStatus
{
    Active = 0,
    Evicted = 1, // Bị đuổi, vi phạm hợp đồng
    Cancelled = 2  ,
    Expired =3 //hợp đồng đã hết hạn
    
}