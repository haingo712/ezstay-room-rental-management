using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using PaymentAPI.DTOs.Requests;
using PaymentAPI.Services.Interfaces;
using Shared.DTOs.Payments.Responses;

namespace PaymentAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BankAccountController : ControllerBase
    {
        private readonly IBankAccountService _bankAccountService;
        private readonly ITokenService _tokenService;

        public BankAccountController(IBankAccountService vnPayService , ITokenService tokenService)
        {
            _bankAccountService = vnPayService;
            _tokenService = tokenService;
        }
        [HttpPost("bank-account")]
        [Authorize(Roles = "Admin, Owner")]
        public async Task<IActionResult> Add([FromBody] CreateBankAccount request)
        {
            var userId = _tokenService.GetUserIdFromClaims(User);
            var result = await _bankAccountService.AddBankAccount(userId, request);
            if (!result.IsSuccess)
                return BadRequest();

            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _bankAccountService.GetById(id);
            return Ok(result);
        }
      
        
        [HttpGet("getAll")]
        [EnableQuery]
        [Authorize(Roles = "Admin, Owner")]
        public IQueryable<BankAccountResponse> GetAll()
        {
            var userId = _tokenService.GetUserIdFromClaims(User);
          return   _bankAccountService.GetAll(userId);
        }
        [Authorize(Roles = "Admin, Owner")]
        [HttpPut("bank-account/{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateBankAccount request)
        {
            var result = await _bankAccountService.UpdateBankAccount(id,request);
            if (!result.IsSuccess)
                return BadRequest();
            return NoContent();
        }
        [Authorize(Roles = "Admin, Owner")]
        [HttpDelete("bank-account/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = _tokenService.GetUserIdFromClaims(User);
            var result = await _bankAccountService.DeleteBankAccount(id);
            return Ok(result);
        }
        [HttpGet("transactions")]
        public async Task<IActionResult> GetTransactions()
        {
            var result = await _bankAccountService.GetTransactionsAsync();
            return Ok(result);
        }
        [HttpGet("owner/{ownerId}")]
        [EnableQuery]
        [Authorize(Roles = "User, Owner, Admin")]
        public IQueryable<BankAccountResponse> GetByOwnerId(Guid ownerId)
        {
            return _bankAccountService.GetAll(ownerId);
        }
        [HttpGet("{ownerId}/getDefault")]
        [EnableQuery]
        public IQueryable<BankAccountResponse> GetAllD(Guid ownerId)
        {
            //     var userId = _tokenService.GetUserIdFromClaims(User);
            return  _bankAccountService.GetDefaultByUserId(ownerId);
        }
        [HttpGet("owner/{ownerId}/bankAccountActive")]
        [EnableQuery]
         [Authorize(Roles = "User, Owner, Admin")]
        public IQueryable<BankAccountResponse> GetByOwnerIdForBill(Guid ownerId, [FromQuery] decimal amount, [FromQuery] string? description)
        {
            return _bankAccountService.GetBankAccountsWithAmount(ownerId, amount, description);
        }
        
       //  [HttpGet("owner/{ownerId}/bill/{billId}")]
       //  [EnableQuery]
       // // [Authorize(Roles = "User, Owner, Admin")]
       // public IQueryable<BankAccountResponse> GetByOwnerIdForBill(Guid ownerId, Guid billId, [FromQuery] decimal amount, [FromQuery] string? description)
       //  {
       //    return _bankAccountService.GetBankAccountsWithAmount(ownerId, billId, amount, description);
       //  }
    }
}
