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
    
    [HttpGet("{contractId}/utilitytype/{utilityType}")]
    [Authorize(Roles = "Owner, User")]
    [EnableQuery]
    public IQueryable<UtilityReadingResponse> GetUtilityReadingByRoomId(Guid contractId, UtilityType utilityType)
    {

        return _utilityReadingService.GetAllByOwnerId(contractId, utilityType);
    }
    
    [HttpGet("all/{contractId}")]
    [Authorize(Roles = "Owner, User")]
    [EnableQuery]
    public IQueryable<UtilityReadingResponse> GetAllUtilityReadingByContractId(Guid contractId)
    {

        return _utilityReadingService.GetAllByContractId(contractId);
    }
    [HttpGet("latest/{contractId}/{utilityType}")]
    public async Task<IActionResult> GetLatestUtilityReadingByContractAndType(Guid contractId, UtilityType utilityType)
    {        
        try
        {
            var result =await _utilityReadingService.GetLastestReading(contractId, utilityType);
            return Ok(result);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(new { message = e.Message });
        }
    }
    [HttpGet("{id}")]
    [Authorize(Roles = "Owner, User")]
    
    public async Task<ActionResult<UtilityReadingResponse>> GetById(Guid id)
    {
        try
        {
            var result = await _utilityReadingService.GetById(id);
            return Ok(result);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(new { message = e.Message });
        }
    }
    
    [HttpPost("{contractId}/utilitytype/{utilityType}")]
    [Authorize(Roles = "Owner")]
    public async Task<ActionResult<UtilityReadingResponse>> Post(Guid contractId, UtilityType utilityType, CreateUtilityReadingContract request)
    {
        try
        {
            var create = await _utilityReadingService.Add(contractId, utilityType, request);
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
    
    [HttpPut("{id}")]
    [Authorize(Roles = "Owner")]
    public async Task<IActionResult> Put(Guid id, UpdateUtilityReading request)
    {
        try
        {
            var update = await _utilityReadingService.Update(id, request);
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
    [HttpPost("{contractId}/utilitytype/{utilityType}/contract")]
    public async Task<ActionResult<UtilityReadingResponse>> AddUtilityReadingContract(Guid contractId,UtilityType utilityType , CreateUtilityReadingContract request)
    {
        try
        {
            var result = await _utilityReadingService.AddUtilityReadingContract(contractId, utilityType, request);
            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.Message });
            }
    
            return Ok(result);
        }
        catch (Exception e)
        {
            return Conflict(new { message = e.Message });
        }
    }
    [HttpPut("{contractId}/utilitytype/{utilityType}/contract")]
    // [Authorize(Roles = "Owner")]
    public async Task<IActionResult> PutContract(Guid contractId, UtilityType utilityType, UpdateUtilityReading request)
    {
        try
        {
            var update = await _utilityReadingService.UpdateUtilityReadingContract(contractId,utilityType , request);
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
    
  

}
