namespace BoardingHouseAPI.DTO.Request
{
    public class RoomCheckResult
    {
        public bool HasBlockedRooms { get; set; }
        public string? Message { get; set; }
    }
}
