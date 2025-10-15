
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using ContractAPI.DTO.Requests;
using ContractAPI.DTO.Response;
using ContractAPI.Enum;
using ContractAPI.Model;
using ContractAPI.Services;
using ContractAPI.Services.Interfaces;
using Shared.DTOs.Contracts.Responses;
using Shared.Enums;

namespace ContractAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class ContractController(IContractService _contractService, ITokenService _tokenService) : ControllerBase
    {
        // private readonly IContractService _contractService;
        // private readonly ITokenService _tokenService;
        //
        //
        // public ContractController(IContractService contractService, ITokenService tokenService)
        // {
        //     _contractService = contractService;
        //     _tokenService = tokenService;
        // }
        
      //  [Authorize(Roles = "User")]
        // [HttpGet("HasContract/{tenantId}/roomId/{roomId}")]
        // public async Task<IActionResult> HasContract(Guid tenantId, Guid roomId)
        // {
        //     var hasContract = await _contractService.HasContractAsync(tenantId, roomId);
        //     return Ok(hasContract); 
        // }
        
        [Authorize(Roles = "Owner")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateContract request)
        {
            var ownerId = _tokenService.GetUserIdFromClaims(User);
            var createContract = await _contractService.Add(ownerId, request);
            if (!createContract.IsSuccess)
                return BadRequest(new { message = createContract.Message });
            return Ok(createContract);
            //CreatedAtAction("GetContractById", new { id = createContract.Data.Id }, createContract);
        }
        [Authorize(Roles = "Owner")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id,[FromBody] UpdateContract request)
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
       // [Authorize(Roles = "Owner")]
       // này m test coi thôi chứ k cần làm nha này là getAll coi thôi 
       [HttpGet]
       [EnableQuery]
       public IQueryable<ContractResponse> GetContracts()
       {
           return _contractService.GetAllQueryable();
       }
       
       [Authorize(Roles = "User")]
       [HttpGet("MyContract")]
       [EnableQuery]
       public IQueryable<ContractResponse> GetContractsByTenantId()
       {
           var tenantId = _tokenService.GetUserIdFromClaims(User);
           return _contractService.GetAllByTenantId(tenantId);
       }
       
       [Authorize(Roles = "Owner")]
       [HttpGet("ByOwnerId")]
       [EnableQuery]
       public IQueryable<ContractResponse> GetContractsByOwnerId()
       {
         var ownerId=  _tokenService.GetUserIdFromClaims(User);
           return _contractService.GetAllByOwnerId(ownerId);
       }
       [Authorize(Roles = "Owner")]
       [HttpGet("ContractStatus")]
       [EnableQuery]
       public IQueryable<ContractResponse> GetContractsStatusByOwnerId(ContractStatus contractStatus)
       {
           var ownerId=  _tokenService.GetUserIdFromClaims(User);
           return _contractService.GetAllByOwnerId(ownerId, contractStatus);
       }
       
        // GET: api/Tenant/5
        // [Authorize(Roles = "Owner, User")]
        [HttpGet("{id}")]
        public async Task<ActionResult<ContractResponse>> GetContractById(Guid id)
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
        [HttpPut("{id}/extendcontract")]
        public async Task<IActionResult> ExtendContract(Guid id, [FromBody] ExtendContractDto dto)
        {
            try
            {
                var result = await _contractService.ExtendContract(id, dto);
                if (!result.IsSuccess)
                    return BadRequest(new { message = result.Message });

                return Ok(result);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
        }
        [Authorize(Roles = "Owner, User")]
        [HttpPut("{id}/cancelcontract")]
        public async Task<IActionResult> CancelContract(Guid id, [FromBody] string reason )
        {
            try
            {
                var result = await _contractService.CancelContract(id, reason);
                if (!result.IsSuccess)
                    return BadRequest(new { message = result.Message });
                return Ok(result);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
        }
        
        // hàm upload hợp đồng 
        [Authorize(Roles = "Owner")]
        [HttpPut("{id}/upload-image")]
        public async Task<IActionResult> CancelContract(Guid id, [FromForm] List<IFormFile> request )
        {
            try
            {
                var result = await _contractService.UploadContractImages(id, request);
                if (!result.IsSuccess)
                    return BadRequest(new { message = result.Message });
                return Ok(result);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
        }
        // làm 
        // // DELETE: api/Tenant/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var result = await _contractService.Delete(id);
                if (!result.IsSuccess)
                    return BadRequest(new { message = result.Message });
                return NoContent();
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
        }
        //  Kiểm tra phòng có hợp đồng không
        [HttpGet("room/{roomId}/exists")]
        public async Task<IActionResult> HasContract(Guid roomId)
        {
            var result = await _contractService.ExistsByRoomId(roomId);
            return Ok(result);
        }
    }
}
