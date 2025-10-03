using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using UtilityBillAPI.DTO.Request;
using UtilityBillAPI.Enum;
using UtilityBillAPI.Service.Interface;

namespace UtilityBillAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilityBillsController : ControllerBase
    {
        private readonly IUtilityBillService _utilityBillService;
        private readonly ITokenService _tokenService; 

        public UtilityBillsController(IUtilityBillService utilityBillService, ITokenService tokenService)
        {
            _utilityBillService = utilityBillService;
            _tokenService = tokenService; 
        }

        // GET: api/UtilityBills
        [HttpGet]
        public IQueryable<UtilityBillDTO> GetUtilityBills()
        {
            return _utilityBillService.GetAll();
        }

        // GET: api/UtilityBills/owner
        /* Get utility bills for the owner with filter by status, roomId
         * 
         * Ex: api/UtilityBills/owner?$filter=Status eq 'Unpaid' and RoomId eq 123e4567-e89b-12d3-a456-426614174000
         * 
         */
        [HttpGet("owner")]
        [EnableQuery]
        [Authorize (Roles = "Owner")]
        public IQueryable<UtilityBillDTO> GetUtilityBillsByOwner()
        {
            var ownerId = _tokenService.GetUserIdFromClaims(User);
            return _utilityBillService.GetAllByOwnerId(ownerId);
        }

        // GET: api/UtilityBills/tenant
        /* Get utility bills for the owner with filter by status, roomId
         * 
         * Ex: api/UtilityBills/owner?$filter=Status eq 'Unpaid' and RoomId eq 123e4567-e89b-12d3-a456-426614174000
         * 
         */
        [HttpGet("tenant")]
        [EnableQuery]
        [Authorize(Roles = "User")]
        public IQueryable<UtilityBillDTO> GetUtilityBillsByTenant()
        {
            var tenantId = _tokenService.GetUserIdFromClaims(User); 
            return _utilityBillService.GetAllByTenantId(tenantId);
        }

        // GET: api/UtilityBills/{id} 
        [HttpGet("{id}")]
        public async Task<ActionResult<UtilityBillDTO>> GetUtilityBill(Guid id)
        {
            try
            {
                var bill = await _utilityBillService.GetByIdAsync(id);
                return Ok(bill);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // POST: api/UtilityBills/generate/{roomId}
        [HttpPost("generate/{roomId}")]
        [Authorize(Roles = "Owner")]
        public async Task<ActionResult<UtilityBillDTO>> GenerateBillForRoom(Guid roomId, [FromQuery] Guid? tenantId)
        {
            var ownerId = _tokenService.GetUserIdFromClaims(User);
            var response = await _utilityBillService.GenerateBillForRoomAsync(ownerId, roomId, tenantId);
            if (!response.IsSuccess)
            {
                return BadRequest(new { message = response.Message });
            }
            return CreatedAtAction(nameof(GetUtilityBill), new { id = response.Data.Id }, response);           
        }

        // PUT: api/UtilityBills/{id}
        [HttpPut("{billId}")]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> Update(Guid billId, [FromForm] UpdateUtilityBillDTO dto)
        {
            var response = await _utilityBillService.UpdateBillAsync(billId, dto);
            if (!response.IsSuccess)
            {
                return BadRequest(new { message = response.Message });
            }
            return Ok(response);
        }

        // PUT: api/UtilityBills/{id}/pay
        [HttpPut("{billId}/pay")]
        [Authorize(Roles = "User, Owner")]
        public async Task<IActionResult> MarkAsPaid(Guid billId, [FromBody] PayBillDTO dto)
        {
            var response = await _utilityBillService.MarkAsPaidAsync(billId, dto.PaymentMethod);
            if (!response.IsSuccess)
            {
                return BadRequest(new { message = response.Message });
            }
            return Ok(response);
        }

        // PUT: api/UtilityBills/{id}/cancel
        [HttpPut("{billId}/cancel")]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> Cancel(Guid billId, [FromBody] CancelBillDTO dto)
        {
            var response = await _utilityBillService.CancelAsync(billId, dto.CancelNote);
            if (!response.IsSuccess)
            {
                return BadRequest(new { message = response.Message });
            }
            return Ok(response);
        }

        // DELETE: api/UtilityBills/{id}
        /*[HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _utilityBillService.DeleteAsync(id);
            if (!response.IsSuccess)
            {
                return BadRequest(new { message = response.Message });
            }
            return Ok(response);
        }*/



    }
}
