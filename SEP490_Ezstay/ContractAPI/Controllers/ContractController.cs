
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
   
    public class ContractController: ControllerBase
    {
        private readonly IContractService _contractService;
        private readonly ITokenService _tokenService;

        public ContractController(IContractService contractService, ITokenService tokenService)
       {
            _contractService = contractService;
            _tokenService = tokenService;
        }
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
   
       [Authorize(Roles = "User")]
       [HttpGet("my-contract")]
       [EnableQuery]
       public IQueryable<ContractResponse> GetContractsByTenantId()
       {
           var tenantId = _tokenService.GetUserIdFromClaims(User);
           return _contractService.GetAllByTenantId(tenantId);
       }
       
       [Authorize(Roles = "Owner, User")]
       [HttpGet("all/owner")]
       [EnableQuery]
       public IQueryable<ContractResponse> GetContractsByOwnerId()
       {
         var ownerId=  _tokenService.GetUserIdFromClaims(User);
           return _contractService.GetAllByOwnerId(ownerId);
       }
       
        // GET: api/Tenant/5
        [Authorize(Roles = "Owner, User")]
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
        public async Task<IActionResult> ExtendContract(Guid id, [FromBody] ExtendContract dto)
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
        [HttpPut("{id}/cancel")]
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
        public async Task<IActionResult> UploadContractImage(Guid id, [FromForm] IFormFileCollection request )
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
      
        [HttpDelete("{id}")]
        [Authorize(Roles = "Owner")]
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
        
        
        // hàm này dùng để 2 người kí 
        // [Authorize(Roles = "User")]
        // [HttpPut("{id}/sign-contract")]
        // public async Task<IActionResult> SignContractUser(Guid id, [FromBody] string userSignature)
        // {
        //     try
        //     {
        //        var role = _tokenService.GetRoleFromClaims(User);
        //         var result = await _contractService.SignContractUser(id, userSignature,role );
        //         if (!result.IsSuccess)
        //             return BadRequest(new { message = result.Message });
        //         return Ok(result);
        //     }
        //     catch (KeyNotFoundException e)
        //     {
        //         return NotFound(new { message = e.Message });
        //     }
        // }
        // hàm này dùng để 2 người kí 
        [Authorize(Roles = "User")]
        [HttpPut("{id}/sign-contract/user")]
        public async Task<IActionResult> SignContractUser(Guid id, [FromBody] string userSignature)
        {
            try
            {
                var user = _tokenService.GetUserIdFromClaims(User);
                var result = await _contractService.SignContractUser(id, userSignature, user);
                if (!result.IsSuccess)
                    return BadRequest(new { message = result.Message });
                return Ok(result);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
        }
        [Authorize(Roles = "Owner")]
        [HttpPut("{id}/sign-contract/owner")]
        public async Task<IActionResult> SignContractOwner(Guid id, [FromBody] string ownerSignature )
        {
            try
            {
                var owner = _tokenService.GetUserIdFromClaims(User);
                var result = await _contractService.SignContractOwner(id, ownerSignature, owner);
                if (!result.IsSuccess)
                    return BadRequest(new { message = result.Message });
                return Ok(result);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
        }
        
        [Authorize(Roles = "User")]
        [HttpPost("{ownerId}/rental-request/{roomId}")]
        public async Task<IActionResult> Post(Guid ownerId, Guid roomId, [FromBody] CreateRentalRequest request)
        {
            var userId = _tokenService.GetUserIdFromClaims(User);

            var result = await _contractService.Add(ownerId,userId, roomId, request);

            if (!result.IsSuccess)
                return BadRequest(new { message = result.Message });
            return Ok(result);
        }

        [Authorize(Roles = "Owner")] 
        [HttpGet("rental-requests/owner")]
        public async Task<IActionResult> GetAllRentalRequestByOwner()
        {
            var ownerId = _tokenService.GetUserIdFromClaims(User);
            var result = await _contractService.GetAllRentalByOwnerIdWithUserInfoAsync(ownerId);
            return Ok(result);
        }
        
        [Authorize(Roles = "User")]
        [HttpGet("rental-requests/user")]
        public async Task<IActionResult> GetAllRentalRequestByUser()
        {
            var userId = _tokenService.GetUserIdFromClaims(User);
            var result = await _contractService.GetAllRentalByUserIdWithOwnerInfoAsync(userId);
            return Ok(result);
        }
        
        /// <summary>
        /// Get all tenants (primary and cohabitants) for owner
        /// Returns list of all IdentityProfile from Active contracts
        /// </summary>
        [Authorize(Roles = "Owner")]
        [HttpGet("tenants")]
        public async Task<IActionResult> GetAllTenants()
        {
            var ownerId = _tokenService.GetUserIdFromClaims(User);
            var result = await _contractService.GetAllTenantsAsync(ownerId);
            return Ok(result);
        }
    }
}
