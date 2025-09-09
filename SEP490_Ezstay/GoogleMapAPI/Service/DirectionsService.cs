using System.Text.Json;
using GoogleMapAPI.Dto;
using GoogleMapAPI.Service.Interface;

namespace GoogleMapAPI.Service;

public class DirectionsService: IDirectionsService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public DirectionsService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _apiKey = config["GoogleMaps:ApiKey"];
    }

    public async Task<DirectionsResponseDto?> GetDirectionsAsync(string origin, string destination)
    // public async Task<DirectionsResponseDto?> GetDirectionsAsync(string originLat, string originLng, string destLat, string destLng)
    {
         var url = $"https://maps.googleapis.com/maps/api/directions/json?origin={Uri.EscapeDataString(origin)}&destination={Uri.EscapeDataString(destination)}&key={_apiKey}";
         var res = await _httpClient.GetStringAsync(url);
        // var url = "https://routes.googleapis.com/directions/v2:computeRoutes";
        //
        // var body = new
        // {
        //     origin = new { latLng = new { latitude = double.Parse(originLat), longitude = double.Parse(originLng) } },
        //     destination = new { latLng = new { latitude = double.Parse(destLat), longitude = double.Parse(destLng) } },
        //     travelMode = "DRIVE"
        // };
    
       // var response = await _httpClient.PostAsJsonAsync(url, body);
       // var resString = await response.Content.ReadAsStringAsync();
    
        using var doc = JsonDocument.Parse(res);
        
       // using var doc = JsonDocument.Parse(res);
        var status = doc.RootElement.GetProperty("status").GetString();
        if (status != "OK")
        {
            Console.WriteLine("Google API status: " + status);
            return null;
        }
    
        var route = doc.RootElement.GetProperty("routes")[0];
        var leg = route.GetProperty("legs")[0];
    
        return new DirectionsResponseDto
        {
            DistanceKm = leg.GetProperty("distance").GetProperty("value").GetDouble() / 1000,
            DurationMin = leg.GetProperty("duration").GetProperty("value").GetDouble() / 60,
            Polyline = route.GetProperty("overview_polyline").GetProperty("points").GetString() ?? ""
        };
    }
 // public async Task<DirectionsResponseDto?> GetDirectionsAsync(string origin, string destination)
 //    {
 //        // 1️⃣ Convert địa chỉ -> lat/lng (Places API)
 //        var originLatLng = await GetLatLngAsync(origin);
 //        var destinationLatLng = await GetLatLngAsync(destination);
 //        if (originLatLng == null || destinationLatLng == null) return null;
 //
 //        // 2️⃣ Gọi Routes API
 //        var requestBody = new
 //        {
 //            origin = new { latLng = new { latitude = originLatLng.Value.Lat, longitude = originLatLng.Value.Lng } },
 //            destination = new { latLng = new { latitude = destinationLatLng.Value.Lat, longitude = destinationLatLng.Value.Lng } },
 //            travelMode = "DRIVE"
 //        };
 //
 //        var res = await _httpClient.PostAsJsonAsync("https://routes.googleapis.com/directions/v2:computeRoutes", requestBody);
 //        using var json = JsonDocument.Parse(await res.Content.ReadAsStringAsync());
 //        var routes = json.RootElement.GetProperty("routes");
 //        if (routes.GetArrayLength() == 0) return null;
 //
 //        var leg = routes[0].GetProperty("legs")[0];
 //        return new DirectionsResponseDto
 //        {
 //            DistanceKm = leg.GetProperty("distance").GetProperty("value").GetDouble() / 1000,
 //            DurationMin = leg.GetProperty("duration").GetProperty("value").GetDouble() / 60,
 //            Polyline = routes[0].GetProperty("overview_polyline").GetProperty("points").GetString() ?? ""
 //        };
 //    }
 //
 //    private async Task<(double Lat, double Lng)?> GetLatLngAsync(string address)
 //    {
 //        var url = $"https://maps.googleapis.com/maps/api/geocode/json?address={Uri.EscapeDataString(address)}&key={_apiKey}";
 //        using var doc = JsonDocument.Parse(await _httpClient.GetStringAsync(url));
 //        if (doc.RootElement.GetProperty("status").GetString() != "OK") return null;
 //
 //        var loc = doc.RootElement.GetProperty("results")[0].GetProperty("geometry").GetProperty("location");
 //        return (loc.GetProperty("lat").GetDouble(), loc.GetProperty("lng").GetDouble());
 //    }
 
 public async Task<DirectionsResponseDto?> GetDirectionsAsync(double originLat, double originLng,
     double destLat, double destLng)
 {
     var url = $"http://router.project-osrm.org/route/v1/driving/{originLng},{originLat};{destLng},{destLat}?overview=full&geometries=polyline";

     var response = await _httpClient.GetAsync(url);
     if (!response.IsSuccessStatusCode)
     {
         Console.WriteLine("OSRM API Error: " + await response.Content.ReadAsStringAsync());
         return null;
     }

     using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
     var routes = json.RootElement.GetProperty("routes");
     if (routes.GetArrayLength() == 0) return null;

     var route = routes[0];
     return new DirectionsResponseDto
     {
         DistanceKm = route.GetProperty("distance").GetDouble() / 1000,  // meters → km
         DurationMin = route.GetProperty("duration").GetDouble() / 60,   // seconds → minutes
         Polyline = route.GetProperty("geometry").GetString() ?? ""
     };
 }
 // public async Task<DirectionsResponseDto?> GetDirectionsAsync(string originAddress, string destinationAddress)
 // {
 //     async Task<(double Lat, double Lng)?> GetLatLng(string address)
 //     {
 //         var url = $"https://maps.googleapis.com/maps/api/geocode/json?address={Uri.EscapeDataString(address)}&key={_apiKey}";
 //         using var doc = JsonDocument.Parse(await _httpClient.GetStringAsync(url));
 //         if (doc.RootElement.GetProperty("status").GetString() != "OK") return null;
 //
 //         var loc = doc.RootElement.GetProperty("results")[0].GetProperty("geometry").GetProperty("location");
 //         return (loc.GetProperty("lat").GetDouble(), loc.GetProperty("lng").GetDouble());
 //     }
 //
 //     var origin = await GetLatLng(originAddress);
 //     var dest = await GetLatLng(destinationAddress);
 //     if (origin == null || dest == null) return null;
 //
 //     var requestBody = new
 //     {
 //         origin = new { latLng = new { latitude = origin.Value.Lat, longitude = origin.Value.Lng } },
 //         destination = new { latLng = new { latitude = dest.Value.Lat, longitude = dest.Value.Lng } },
 //         travelMode = "DRIVE"
 //     };
 //
 //     var res = await _httpClient.PostAsJsonAsync("https://routes.googleapis.com/directions/v2:computeRoutes", requestBody);
 //     using var json = JsonDocument.Parse(await res.Content.ReadAsStringAsync());
 //
 //     var routes = json.RootElement.GetProperty("routes");
 //     if (routes.GetArrayLength() == 0) return null;
 //
 //     var leg = routes[0].GetProperty("legs")[0];
 //     return new DirectionsResponseDto
 //     {
 //         DistanceKm = leg.GetProperty("distance").GetProperty("value").GetDouble() / 1000,
 //         DurationMin = leg.GetProperty("duration").GetProperty("value").GetDouble() / 60,
 //         Polyline = routes[0].GetProperty("overview_polyline").GetProperty("points").GetString() ?? ""
 //     };
 // }

}
