using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using RoomAPI.Enum;

namespace RoomAPI.DTO.Request;

public class RoomDto
{
    [Key]
    public Guid Id { get; set; }
    public Guid HouseId { get; set; }
    public Guid HouseLocationId { get; set; }
    public string RoomName { get; set; } 
    public decimal Area { get; set; }
    public decimal Price { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public RoomStatus RoomStatus { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}