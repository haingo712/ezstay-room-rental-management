
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using TenantAPI.DTO.Requests;
using TenantAPI.DTO.Response;
using TenantAPI.Model;
using TenantAPI.Services;
using TenantAPI.Services.Interface;

namespace TenantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class TenantController : ControllerBase
    {
        private readonly ITenantService _tenantService;
        private readonly ITokenService _tokenService;

        public TenantController(ITenantService tenantService, ITokenService tokenService)
        {
            _tenantService = tenantService;
            _tokenService = tokenService;
        }
        
       // [Authorize(Roles = "Owner")]
       // này m test coi thôi chứ k cần làm nha này là getAll coi thôi 
        [HttpGet]
        [EnableQuery]
        public  IQueryable<TenantDto> GetTenants()
        {
            return  _tenantService.GetAllQueryable();
        }
        
        
        [Authorize(Roles = "Owner")]
        [HttpGet("ByUserId/{userId}")]
        [EnableQuery]
        public  IQueryable<TenantDto> GetTenantsByUserId(Guid userId )
        {
            return  _tenantService.GetAllByUserId(userId);
        }
        
        // [Authorize(Roles = "Owner")]
        // [HttpGet("ByRoomId/{roomId}")]
        // [EnableQuery]
        // public  IQueryable<TenantDto> GetTenantsByRoomId(int roomId )
        // {
        //
        //     return  _tenantService.GetAllroomId);
        // }

        // GET: api/Tenant/5
        [Authorize(Roles = "Owner, User")]
        [HttpGet("{id}")]
        public async Task<ActionResult<TenantDto>> GetTenantById(Guid id)
        {
            try
            {
                var tenant = await _tenantService.GetByIdAsync(id);
                return Ok(tenant);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
        }
        [Authorize(Roles = "Owner")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTenant(Guid id, UpdateTenantDto request)
        {
            try
            {
               var result = await _tenantService.UpdateAsync(id, request);
                if (!result.IsSuccess  )
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
        
         [Authorize(Roles = "Owner")]
        [HttpPost]
        public async Task<ActionResult<TenantDto>> PostTenant(CreateTenantDto request)
        {
            try
            { 
                var ownerId = _tokenService.GetUserIdFromClaims(User);
             var createTenant=  await _tenantService.AddAsync(ownerId, request);
             if (!createTenant.IsSuccess ) // || createTenant.Data == null)
             {
                 return BadRequest(new { message = createTenant.Message });
             }
                return CreatedAtAction("GetTenantById", new { id = createTenant.Data.Id }, createTenant);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
        }
        [HttpPut("{id}/extendtenant")]
        public async Task<IActionResult> Extend(Guid id, [FromBody] ExtendTenantDto dto)
        {
            try
            {
                var result = await _tenantService.ExtendContractAsync(id, dto);
                if (!result.IsSuccess  )
                {
                    return BadRequest(new { message = result.Message });
                }
                return Ok(result);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
            
        }

        // // DELETE: api/Tenant/5
        // [HttpDelete("{id}")]
        // public async Task<IActionResult> DeleteTenant(int id)
        // {
        //     try
        //     {
        //         await _tenantService.DeleteAsync(id);
        //         return NoContent();
        //     }
        //     catch (KeyNotFoundException e)
        //     {
        //         return NotFound(new { message = e.Message });
        //     }
        // }
    }
}
