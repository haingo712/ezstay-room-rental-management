using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using UtilityReadingAPI.DTO.Request;
using UtilityReadingAPI.DTO.Response;
using UtilityReadingAPI.Enum;
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

         [HttpGet("/odata/{roomId}")]
        [EnableQuery]
        public IQueryable<UtilityReadingResponseDto> GetUtilityReadingByRoomId(Guid roomId, UtilityType utilityType)
        {
           
            return _utilityReadingService.GetAllByOwnerId(roomId, utilityType);
        }

        // GET: api/Amenity/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UtilityReadingResponseDto>> GetUtilityReading(Guid id)
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
        
       
        [HttpPost("{roomId}")]
          [Authorize(Roles = "Owner")]
        public async Task<ActionResult<UtilityReadingResponseDto>> PostUtilityReading(Guid roomId, CreateUtilityReadingDto request)
        {
            try
            {
                var create =   await  _utilityReadingService.AddAsync(roomId, request);
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
        
        // PUT: api/Amenity/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Owner")]
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
        
     
        // DELETE: api/Amenity/5
        [Authorize(Roles = "Owner")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUtilityReading(Guid id)
        {
            try
            { 
                await _utilityReadingService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
        }
        
}
