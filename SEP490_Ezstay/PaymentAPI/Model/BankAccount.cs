using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PaymentAPI.Model;

public class BankAccount
{
    [BsonId]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid UserId { get; set; } 
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    
    public Guid BankGatewayId { get; set; }
    public string AccountNumber { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public string ImageQR { get; set; }
    
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

}