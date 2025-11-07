using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Shared.Enums;

namespace PaymentAPI.Model;

public class Payment
{
    [BsonId]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid Id { get; set; } = Guid.NewGuid();

    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid UtilityBillId { get; set; }

    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid TenantId { get; set; } 

    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid OwnerId { get; set; }

    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid BankAccountId { get; set; } 
    public string? BankAccountNumber { get; set; } // Số tài khoản lấy từ BankAccount.AccountNumber của CHỦ TRỌ (cho online payment)

    public decimal Amount { get; set; }
    
    public PaymentMethod PaymentMethod { get; set; } 
    public PaymentStatus Status { get; set; }

    public string? TransactionId { get; set; } // Mã giao dịch từ SePay (TransactionNumber)
    public string? TransactionContent { get; set; } // Nội dung chuyển khoản (Description từ SePay)

    public string? ReceiptImageUrl { get; set; } // Ảnh biên lai / ảnh xác nhận (cho offline payment)
    public string? SePayResponse { get; set; } // Lưu toàn bộ JSON response từ SePay
    
    public string? BankBrandName { get; set; } // Tên ngân hàng từ SePay (để biết tenant chuyển từ bank nào)
    public DateTime? TransactionDate { get; set; } // Thời gian giao dịch thực tế từ SePay
    
    public DateTime CreatedDate { get; set; } 
    
    public DateTime? UpdatedDate { get; set; }

    public DateTime? CompletedDate { get; set; } 
    public string? Notes { get; set; } 

    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid? ApprovedBy { get; set; } // Owner ID người duyệt
    
    [BsonRepresentation(BsonType.DateTime)]
    public DateTime? ApprovedAt { get; set; }
    
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid? RejectedBy { get; set; } // Owner ID người từ chối
    
    [BsonRepresentation(BsonType.DateTime)]
    public DateTime? RejectedAt { get; set; }
    
    public string? RejectionReason { get; set; } // Lý do từ chối
}