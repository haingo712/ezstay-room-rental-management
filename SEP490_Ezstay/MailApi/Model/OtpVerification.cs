namespace MailApi.Model;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class OtpVerification
{
    [BsonId]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid Id { get; set; } = Guid.NewGuid();
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid ContractId { get; set; }
    
    public string Email { get; set; } 
    
   
    public string OtpCode { get; set; }
    
    public DateTime ExpireAt { get; set; }
    
  
    public bool IsUsed { get; set; }
    
   
   
}
