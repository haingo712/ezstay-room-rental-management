namespace GoogleMapAPI.Dto;

public class DirectionsResponseDto
{
    public double DistanceKm { get; set; }
    public double DurationMin { get; set; }
    public string Polyline { get; set; }
}