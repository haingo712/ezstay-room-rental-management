
using ContractAPI.DTO.Requests;
using ContractAPI.DTO.Response;
using ContractAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;


namespace ContractAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class IdentityProfileController : ControllerBase
    {
        private readonly IIdentityProfileService _identityProfileService;
        private readonly ITokenService _tokenService;

        public IdentityProfileController(IIdentityProfileService identityProfileService, ITokenService tokenService)
        {
            _identityProfileService = identityProfileService;
            _tokenService = tokenService;
        }
       // này m test coi thôi chứ k cần làm nha này là getAll coi thôi 
        [HttpGet]
        [EnableQuery]
        public  IQueryable<IdentityProfileResponseDto> GetAll()
        {
            return  _identityProfileService.GetAllQueryable();
        }
        [Authorize(Roles = "Owner, User")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
         
            try
            {
                var tenant = await _identityProfileService.GetByIdAsync(id);
                return Ok(tenant);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
        }
        
        // [Authorize(Roles = "Owner")]
        [HttpGet("ByTenantId/{tenantId}")]
        [EnableQuery]
        public  IQueryable<IdentityProfileResponseDto> GetTenantsByRoomId(Guid tenantId)
        {
            return  _identityProfileService.GetAllByTenantId(tenantId);
        }
        
        //  [Authorize(Roles = "Owner")]
        // [HttpPost]
        // public async Task<IActionResult> Create([FromBody] CreateIdentityProfileDto request)
        // {
        //     try
        //     { 
        //         var ownerId = _tokenService.GetUserIdFromClaims(User);
        //         var createTenant=  await _identityProfileService.AddAsync(request);
        //         if (!createTenant.IsSuccess )
        //         {
        //             return BadRequest(new { message = createTenant.Message });
        //         }
        //         return CreatedAtAction("GetById", new { id = createTenant.Data.Id }, createTenant);
        //     }
        //     catch (KeyNotFoundException e)
        //     {
        //         return NotFound(new { message = e.Message });
        //     }
        // }
        [Authorize(Roles = "Owner")]
        [HttpPost("{contractId}")]
        public async Task<IActionResult> Create(Guid contractId, [FromBody] CreateIdentityProfileDto request)
        {
            try
            { 
                var ownerId = _tokenService.GetUserIdFromClaims(User);
                var createTenant=  await _identityProfileService.AddAsync(contractId, request);
                if (!createTenant.IsSuccess )
                {
                    return BadRequest(new { message = createTenant.Message });
                }
                return CreatedAtAction("GetById", new { id = createTenant.Data.Id }, createTenant);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
        }
        // [Authorize(Roles = "Owner")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateIdentityProfileDto request)
        {
            try
            {
                var result = await _identityProfileService.UpdateAsync(id, request);
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
        // [Authorize(Roles = "Owner")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _identityProfileService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
      
    }
}