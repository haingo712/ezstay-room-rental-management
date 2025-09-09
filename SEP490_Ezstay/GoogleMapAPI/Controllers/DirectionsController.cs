
using GoogleMapAPI.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace GoogleMapAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class DirectionsController : ControllerBase
{
    private readonly IDirectionsService _directionsService;

    public DirectionsController(IDirectionsService directionsService)
    {
        _directionsService = directionsService;
    }
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] string origin, [FromQuery] string destination)
    {
        var result = await _directionsService.GetDirectionsAsync(origin, destination);
        if (result == null) return BadRequest("Không lấy được chỉ đường");
        return Ok(result);
    }
    // [HttpGet]
    // public async Task<IActionResult> Get([FromQuery] double originLat, [FromQuery] double originLng,
    //     [FromQuery] double destLat, [FromQuery] double destLng)
    // {
    //     var result = await _directionsService.GetDirectionsAsync(originLat, originLng, destLat, destLng);
    //     if (result == null) return BadRequest("Không lấy được chỉ đường");
    //     return Ok(result);
    // }
    // [HttpGet]
    // public async Task<IActionResult> Get([FromQuery] string origin, [FromQuery] string destination)
    // {
    //     var result = await _directionsService.GetDirectionsAsync(origin, destination);
    //     if (result == null) return BadRequest("Không lấy được chỉ đường");
    //     return Ok(result);
    // }
    // [HttpGet]
    // public async Task<IActionResult> Get([FromQuery] double originLat, [FromQuery] double originLng,
    //     [FromQuery] double destLat, [FromQuery] double destLng)
    // {
    //     var result = await _directionsService.GetDirectionsAsync(originLat, originLng, destLat, destLng);
    //     if (result == null) return BadRequest("Không lấy được chỉ đường");
    //     return Ok(result);
    // }

}