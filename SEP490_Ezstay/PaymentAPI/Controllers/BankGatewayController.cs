
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using PaymentAPI.Services;
using PaymentAPI.Services.Interfaces;
using Shared.DTOs.Payments.Responses;

namespace PaymentAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BankGatewayController : ControllerBase
{
    private readonly IBankGatewayService _bankGatewayService;
    private readonly ITokenService _tokenService;

    public BankGatewayController(IBankGatewayService bankGatewayService, ITokenService tokenService)
    {
        _bankGatewayService = bankGatewayService;
        _tokenService = tokenService;
    }

    [HttpPost("sync")]
    public async Task<IActionResult> LoadBankGateway()
    {
        var result = await _bankGatewayService.SyncFromVietQR();
        return Ok(result);
    }
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> HidenBankGateway(Guid id,  bool isActive)
    {
        return Ok(await _bankGatewayService.HiddenBankGateway(id, isActive));
    }
    [HttpGet]
    [EnableQuery]
    public IQueryable<BankGatewayResponse> GetAll()
    {
        return  _bankGatewayService.GetAllBankGateways();
    }
 
}
