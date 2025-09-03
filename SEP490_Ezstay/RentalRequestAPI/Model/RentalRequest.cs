using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using RentalRequestAPI.Enum;

namespace RentalRequestAPI.Model;

public class RentalRequest
{
    [BsonId] 
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [BsonGuidRepresentation(GuidRepresentation.Standard)] 
    public Guid? UserId { get; set; }
    
    [BsonGuidRepresentation(GuidRepresentation.Standard)] 
    public Guid? OwnerId { get; set; }
    
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid? GuestId { get; set; }
    
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid RoomId { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public DateTime? RejectedAt { get; set; }
    public string? FullName { get; set; }
    public string NumberPhone { get; set; }
    public string? Notes { get; set; }
    public RequestStatus Status { get; set; } 
}
