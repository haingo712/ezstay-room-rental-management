using GoogleMapAPI.Dto;

namespace GoogleMapAPI.Service.Interface;

public interface IDirectionsService
{
   // Task<DirectionsResponseDto?> GetDirectionsAsync(string origin, string destination);
   //Task<DirectionsResponseDto?> GetDirectionsAsync(string originLat, string originLng, string destLat, string destLng);
   Task<DirectionsResponseDto?> GetDirectionsAsync(string origin, string destination);

   Task<DirectionsResponseDto?> GetDirectionsAsync(double originLat, double originLng,
       double destLat, double destLng);
}