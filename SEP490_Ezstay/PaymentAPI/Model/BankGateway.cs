using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PaymentAPI.Model;

public class BankGateway
{
    [BsonId]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid Id { get; set; } = Guid.NewGuid();
    public string BankName { get; set; }   
    public string FullName { get; set; } 
    public string Logo { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}