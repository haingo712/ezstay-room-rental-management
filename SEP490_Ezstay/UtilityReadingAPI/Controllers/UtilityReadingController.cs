using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Shared.DTOs.UtilityReadings.Responses;
using Shared.Enums;
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

    [HttpGet("/odata/{roomId}")]
    [EnableQuery]
    public IQueryable<UtilityReadingResponse> GetUtilityReadingByRoomId(Guid roomId, UtilityType utilityType)
    {

        return _utilityReadingService.GetAllByOwnerId(roomId, utilityType);
    }


    [HttpGet("lastest/{roomId}")]
    public ActionResult<UtilityReadingResponse> GetLastestUtilityReadingByRoomIdAndType(Guid roomId, UtilityType utilityType)
    {        
        try
        {
            var amenity = _utilityReadingService.GetLastestReading(roomId, utilityType);
            return Ok(amenity);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(new { message = e.Message });
        }
    }

    // GET: api/Amenity/5
    [HttpGet("{id}")]
    public async Task<ActionResult<UtilityReadingResponse>> GetById(Guid id)
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


    // [HttpPost("{roomId}")]
    // [Authorize(Roles = "Owner")]
    // public async Task<ActionResult<UtilityReadingResponse>> PostUtilityReading(Guid roomId, CreateUtilityReading request)
    // {
    //     try
    //     {
    //         var create = await _utilityReadingService.AddAsync(roomId, request);
    //         if (!create.IsSuccess)
    //         {
    //             return BadRequest(new { message = create.Message });
    //         }
    //         return CreatedAtAction("GetById", new { id = create.Data.Id }, create);
    //     }
    //     catch (Exception e)
    //     {
    //         return Conflict(new { message = e.Message });
    //     }
    // }
  
   
    [HttpPost("{roomId}/utilitytype/{utilityType}/contract")]
    public async Task<ActionResult<UtilityReadingResponse>> PostUtilityReadindContract(Guid roomId,UtilityType utilityType , CreateUtilityReadingContract request)
    {
        try
        {
            var create = await _utilityReadingService.AddUtilityReadingContract(roomId, utilityType, request);
            if (!create.IsSuccess)
            {
                return BadRequest(new { message = create.Message });
            }

            return Ok(create);
        }
        catch (Exception e)
        {
            return Conflict(new { message = e.Message });
        }
    }

    [HttpPost("{roomId}/utilitytype/{utilityType}")]
    [Authorize(Roles = "Owner")]
    public async Task<ActionResult<UtilityReadingResponse>> Post(Guid roomId, UtilityType utilityType, CreateUtilityReadingContract request)
    {
        try
        {
            var create = await _utilityReadingService.AddAsync(roomId, utilityType, request);
            if (!create.IsSuccess)
            {
                return BadRequest(new { message = create.Message });
            }
            return CreatedAtAction("GetById", new { id = create.Data.Id }, create);
        }
        catch (Exception e)
        {
            return Conflict(new { message = e.Message });
        }
    }

    // PUT: api/Amenity/5
    [HttpPut("{id}")]
    [Authorize(Roles = "Owner")]
    public async Task<IActionResult> Put(Guid id, UpdateUtilityReading request)
    {
        try
        {
            var update = await _utilityReadingService.UpdateAsync(id, request);
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

   
    [HttpPut("{roomId}/utilitytype/{utilityType}/contract")]
    // [Authorize(Roles = "Owner")]
    public async Task<IActionResult> PutContract(Guid roomId, UtilityType utilityType, UpdateUtilityReading request)
    {
        try
        {
            var update = await _utilityReadingService.UpdateContract(roomId,utilityType , request);
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
    public async Task<IActionResult> Delete(Guid id)
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
