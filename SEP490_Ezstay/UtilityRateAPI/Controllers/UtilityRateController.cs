// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.OData.Query;
// using UtilityRateAPI.DTO.Request;
// using UtilityRateAPI.DTO.Response;
// using UtilityRateAPI.Model;
// using UtilityRateAPI.Service.Interface;
//
// namespace UtilityRateAPI.Controllers;
//
// [ApiController]
// [Route("api/[controller]")]
// public class UtilityRateController : ControllerBase
// {
//     private readonly IUtilityRateService _utilityRateService;
//
//     public UtilityRateController(IUtilityRateService utilityRateService)
//     {
//         _utilityRateService = utilityRateService;
//     }
//     [HttpGet("odata")]
//     [EnableQuery]
//     public IQueryable<UtilityRateDto> GetAllOdata()
//     {
//         return  _utilityRateService.GetAllOdata();
//     }
//     
//     [HttpGet]
//     public async Task<ActionResult<ApiResponse<IEnumerable<UtilityRate>>>> GetAll()
//     {
//         var rates = await _utilityRateService.GetAll();
//         return Ok(rates);
//     }
//     [HttpGet("ByOwnerId/{ownerId}")]
//     public async Task<ActionResult<IEnumerable<UtilityRateDto>>> GetAllByOwnerId(Guid ownerId)
//     {
//         var rates = await _utilityRateService.GetAllByOwnerId(ownerId);
//         return Ok(rates);
//     }
//     [HttpGet("ByOwnerId/odata/{ownerId}")]
//     [EnableQuery]
//     public IQueryable<UtilityRateDto> GetAllByOwnerIdOdta(Guid ownerId)
//     {
//      return _utilityRateService.GetAllByOwnerIdOdata(ownerId);
//     }
//     
//     [HttpGet("{id}")]
//     public async Task<ActionResult<UtilityRateDto>> GetById(Guid id)
//     {
//         var rate = await _utilityRateService.GetByIdAsync(id);
//         if (rate == null)
//             return NotFound();
//         return Ok(rate);
//     }
//
//     [HttpPost]
//     public async Task<ActionResult<UtilityRateDto>> Create(CreateUtilityRateDto request)
//     {
//         var rate = await _utilityRateService.AddAsync(request);
//         if (!rate.IsSuccess)
//         {
//             return BadRequest(new { message = rate.Message });
//         }
//         return CreatedAtAction(nameof(GetById), new { id = rate.Data.Id }, rate);
//     }
//
//     [HttpPut("{id}")]
//     
//     public async Task<IActionResult> Update(Guid id, UpdateUtilityRateDto request)
//     {
//         try
//         {
//             var result =   await _utilityRateService.UpdateAsync(id, request);
//             if (!result.IsSuccess)
//             {
//                 return BadRequest(new { message = result.Message });
//             }
//             return NoContent();
//         }
//         catch (KeyNotFoundException e)
//         {
//             return NotFound(new { message = e.Message });
//         }
//     }
//
//     // [HttpDelete("{id}")]
//     // public async Task<IActionResult> Delete(Guid id)
//     // {
//     //     var success = await _utilityRateService.DeleteAsync(id);
//     //     if (!success)
//     //         return NotFound();
//     //     return NoContent();
//     // }
// }

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using UtilityRateAPI.DTO.Request;
using UtilityRateAPI.DTO.Response;
using UtilityRateAPI.Model;
using UtilityRateAPI.Service.Interface;

namespace UtilityRateAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UtilityRateController : ControllerBase
{
    private readonly IUtilityRateService _utilityRateService;

    public UtilityRateController(IUtilityRateService utilityRateService)
    {
        _utilityRateService = utilityRateService;
    }
    [HttpGet("odata")]
    [EnableQuery]
    public IQueryable<UtilityRateDto> GetAllOdata()
    {
        return  _utilityRateService.GetAllOdata();
    }
    
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<UtilityRate>>>> GetAll()
    {
        var rates = await _utilityRateService.GetAll();
        return Ok(rates);
    }
    [HttpGet("ByOwnerId")]
    [Authorize(Roles = "Owner")]
    public async Task<ActionResult<IEnumerable<UtilityRateDto>>> GetAllByOwnerId( )
    {
        var rates = await _utilityRateService.GetAllByOwnerId();
        return Ok(rates);
    }
    [HttpGet("ByOwnerId/odata")]
    [Authorize(Roles = "Owner")]
    [EnableQuery]
    public IQueryable<UtilityRateDto> GetAllByOwnerIdOdta( )
    {
     return _utilityRateService.GetAllByOwnerIdOdata();
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<UtilityRateDto>> GetById(Guid id)
    {
        var rate = await _utilityRateService.GetByIdAsync(id);
        if (rate == null)
            return NotFound();
        return Ok(rate);
    }

    [HttpPost]
    [Authorize(Roles = "Owner")]
    public async Task<ActionResult<UtilityRateDto>> Create(CreateUtilityRateDto request)
    {
        var rate = await _utilityRateService.AddAsync(request);
        if (!rate.IsSuccess)
        {
            return BadRequest(new { message = rate.Message });
        }
        return CreatedAtAction(nameof(GetById), new { id = rate.Data.Id }, rate);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Owner")]
    public async Task<IActionResult> Update(Guid id, UpdateUtilityRateDto request)
    {
        try
        {
            var result =   await _utilityRateService.UpdateAsync(id, request);
            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.Message });
            }
            return NoContent();
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(new { message = e.Message });
        }
    }

    // [HttpDelete("{id}")]
    // public async Task<IActionResult> Delete(Guid id)
    // {
    //     var success = await _utilityRateService.DeleteAsync(id);
    //     if (!success)
    //         return NotFound();
    //     return NoContent();
    // }
}