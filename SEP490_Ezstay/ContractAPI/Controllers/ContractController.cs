
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using ContractAPI.DTO.Requests;
using ContractAPI.DTO.Response;
using ContractAPI.Model;
using ContractAPI.Services;
using ContractAPI.Services.Interfaces;

namespace ContractAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class ContractController : ControllerBase
    {
        private readonly IContractService _contractService;
        private readonly ITokenService _tokenService;

        public ContractController(IContractService contractService, ITokenService tokenService)
        {
            _contractService = contractService;
            _tokenService = tokenService;
        }
        
       // [Authorize(Roles = "Owner")]
       // này m test coi thôi chứ k cần làm nha này là getAll coi thôi 
       [HttpGet]
       [EnableQuery]
       public IQueryable<ContractResponseDto> GetContracts()
       {
           return _contractService.GetAllQueryable();
       }
        
       [Authorize(Roles = "Owner")]
       [HttpGet("ByTenantId/{tenantId}")]
       [EnableQuery]
       public IQueryable<ContractResponseDto> GetContractsByTenantId(Guid tenantId)
       {
           return _contractService.GetAllByTenantId(tenantId);
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
        public async Task<ActionResult<ContractResponseDto>> GetContractById(Guid id)
        {
            try
            {
                var contract = await _contractService.GetByIdAsync(id);
                return Ok(contract);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
        }

        
        [Authorize(Roles = "Owner")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContract(Guid id, UpdateContractDto request)
        {
            try
            {
               var result = await _contractService.UpdateAsync(id, request);
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
        public async Task<ActionResult<ContractResponseDto>> PostContract(CreateContractDto request)
        {
            try
            {
                var ownerId = _tokenService.GetUserIdFromClaims(User);
                var createContract = await _contractService.AddAsync(ownerId, request);

                if (!createContract.IsSuccess)
                    return BadRequest(new { message = createContract.Message });

                return CreatedAtAction("GetContractById", new { id = createContract.Data.Id }, createContract);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
        }
        [HttpPut("{id}/extendcontract")]
        public async Task<IActionResult> ExtendContract(Guid id, [FromBody] ExtendContractDto dto)
        {
            try
            {
                var result = await _contractService.ExtendContractAsync(id, dto);
                if (!result.IsSuccess)
                    return BadRequest(new { message = result.Message });

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
