namespace BoardingHouseAPI.DTO.Response;

public class OccupancyRateResponse
{
    public Guid OwnerId { get; set; }
    public int TotalRooms { get; set; }
    public int OccupiedRooms { get; set; }
    public double OccupancyRate { get; set; } // Tỉ lệ phòng đã thuê (%)
    public List<BoardingHouseOccupancyDetail> BoardingHouses { get; set; } = new();
}

public class BoardingHouseOccupancyDetail
{
    public Guid BoardingHouseId { get; set; }
    public string HouseName { get; set; } = string.Empty;
    public int TotalRooms { get; set; }
    public int OccupiedRooms { get; set; }
    public double OccupancyRate { get; set; } // Tỉ lệ phòng đã thuê của từng nhà trọ (%)
}