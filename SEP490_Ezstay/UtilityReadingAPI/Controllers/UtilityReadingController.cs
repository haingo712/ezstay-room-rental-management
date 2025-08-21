using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using UtilityReadingAPI.DTO.Request;
using UtilityReadingAPI.Service.Interface;

namespace UtilityReadingAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UtilityReadingController : ControllerBase
{
    private readonly IUtilityReadingService _utilityReadingService;

    public UtilityReadingController(IUtilityReadingService utilityReadingService)
    {
        _utilityReadingService = utilityReadingService;
    }

    // [HttpGet("ByOwnerId/odata/{ownerId}")]
    //     [EnableQuery]
    //     public IQueryable<AmenityDto> GetAmenitiesByOwnerIdOdata(Guid ownerId)
    //     {
    //        
    //         return _amenityService.GetAllByOwnerIdOdata(ownerId);
    //     }
    //     
    //     [HttpGet("ByOwnerId/{ownerId}")]
    //     public async Task<ActionResult<IEnumerable<AmenityDto>>> GetAmenitiesByOwnerId(Guid ownerId)
    //     {
    //         return Ok( await _amenityService.GetAllByOwnerId(ownerId));
    //     }
        

        // GET: api/Amenity/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UtilityReadingDto>> GetUtilityReading(Guid id)
        {
            try
            {
                var amenity = await _utilityReadingService.GetByIdAsync(id);
                    return Ok(amenity);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
        }
        // PUT: api/Amenity/5
        [HttpPut("{id}")]
   //     [Authorize(Roles = "Owner")]
        public async Task<IActionResult> PutUtilityReading(Guid id, UpdateUtilityReadingDto request)
        {
            try
            {
                var update =  await _utilityReadingService.UpdateAsync(id, request);
                if (!update.IsSuccess)
                {
                    return BadRequest(new { message = update.Message });
                }
                return Ok(update);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
        }
        
        [HttpPost]
      //  [Authorize(Roles = "Owner")]
        public async Task<ActionResult<UtilityReadingDto>> PostUtilityReading(CreateUtilityReadingDto request)
        {
            try
            {
                var create =   await  _utilityReadingService.AddAsync(request);
                if (!create.IsSuccess)
                {
                    return BadRequest(new { message = create.Message });
                }
                return CreatedAtAction("GetUtilityReading", new { id = create.Data.Id }, create);
            }
            catch (Exception e)
            {
                return Conflict(new { message = e.Message }); 
            }
          
        }
        // DELETE: api/Amenity/5
        // [HttpDelete("{id}")]
        // public async Task<IActionResult> DeleteAmenity(Guid id)
        // {
        //     try
        //     { 
        //         await _amenityService.DeleteAsync(id);
        //         return NoContent();
        //     }
        //     catch (KeyNotFoundException e)
        //     {
        //         return NotFound(new { message = e.Message });
        //     }
        // }
        
}
