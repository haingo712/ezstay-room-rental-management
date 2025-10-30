﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using UtilityBillAPI.DTO;
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
         * Ex: api/UtilityBills/tenant?$filter=Status eq 'Unpaid' and RoomId eq 123e4567-e89b-12d3-a456-426614174000
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

        // POST: api/UtilityBills/generate/{contractId}
        [HttpPost("generate/{contractId}")]
        [Authorize(Roles = "Owner")]
        public async Task<ActionResult<UtilityBillDTO>> GenerateBillForContract(Guid contractId)
        {
            var response = await _utilityBillService.GenerateUtilityBillAsync(contractId);
            if (!response.IsSuccess)
            {
                return BadRequest(new { message = response.Message });
            }
            return CreatedAtAction(nameof(GetUtilityBill), new { id = response.Data.Id }, response);
        }

        // PUT: api/UtilityBills/{id}/pay
        [HttpPut("{billId}/pay")]
        [Authorize(Roles = "User, Owner")]
        public async Task<IActionResult> MarkAsPaid(Guid billId)
        {
            var response = await _utilityBillService.MarkAsPaidAsync(billId);
            if (!response.IsSuccess)
            {
                return BadRequest(new { message = response.Message });
            }
            return Ok(response);
        }

        // PUT: api/UtilityBills/{id}/cancel
        [HttpPut("{billId}/cancel")]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> Cancel(Guid billId, [FromBody] string? reason)
        {
            var response = await _utilityBillService.CancelAsync(billId, reason);
            if (!response.IsSuccess)
            {
                return BadRequest(new { message = response.Message });
            }
            return Ok(response);
        }      


    }
}
