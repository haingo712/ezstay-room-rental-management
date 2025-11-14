using System.Net.Http.Headers;
using System.Text.Json;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.Extensions.Options;
using PaymentAPI.Config;
using PaymentAPI.DTOs.Requests;
using PaymentAPI.Model;
using PaymentAPI.Repository.Interface;
using PaymentAPI.Services.Interfaces;
using Shared.DTOs;
using Shared.DTOs.Payments.Responses;

namespace PaymentAPI.Services;

public class BankAccountService:IBankAccountService
{
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly IMapper _mapper;
    private readonly HttpClient _http;
    private readonly SePayConfig _settings;
    private readonly ITokenService _tokenService;
   
    public BankAccountService(IBankAccountRepository bankAccountRepository, IMapper mapper, HttpClient httpClient, IOptions<SePayConfig> settings, ITokenService tokenService)
    {
        _bankAccountRepository = bankAccountRepository;
        _mapper = mapper;
        _http = httpClient;
        _settings = settings.Value;
        _tokenService = tokenService;
    }

    public async Task<BankAccountResponse> GetById(Guid id)
    {
     var result=   await _bankAccountRepository.GetById(id); 
     return   _mapper.Map<BankAccountResponse>(result);
    }

    public IQueryable<BankAccountResponse> GetAll(Guid userId)
    {
        var bankAccount = _bankAccountRepository.GetAllAsQueryable(userId);
        return bankAccount.ProjectTo<BankAccountResponse>(_mapper.ConfigurationProvider);
    }
    public IQueryable<BankAccountResponse> GetBankAccountsWithAmount(Guid ownerId, decimal amount, string? description)
    {
        var bankAccounts = _bankAccountRepository.GetDefaultByUserId(ownerId).ToList();
        var result= bankAccounts.Select(c => new BankAccountResponse
        {
            // Id = c.Id,
            UserId = c.UserId,
            BankName = c.BankName,
            AccountNumber = c.AccountNumber,
            Amount = amount, 
            Description = description, 
            ImageQR = $"https://qr.sepay.vn/img?acc={c.AccountNumber}&bank={c.BankName}&amount={amount}&des={description}",
            CreatedAt = c.CreatedAt,
            UpdatedAt = c.UpdatedAt
        });
        return result.AsQueryable();
    }
    // public IQueryable<BankAccountResponse> GetBankAccountsWithAmount(Guid ownerId, Guid billId, decimal amount, string? description = null)
    // {
    //     var bankAccounts = _bankAccountRepository.GetDefaultByUserId(ownerId).ToList();
    //     var encodedDes = description ?? $"BILL {billId.ToString().ToUpper()}";
    //    // var encodedDes = Uri.EscapeDataString(transactionContent);
    //     var result= bankAccounts.Select(c => new BankAccountResponse
    //     {
    //         // Id = c.Id,
    //         UserId = c.UserId,
    //         BankName = c.BankName,
    //         AccountNumber = c.AccountNumber,
    //         Amount = amount, 
    //         Description = encodedDes, 
    //         ImageQR = $"https://qr.sepay.vn/img?acc={c.AccountNumber}&bank={c.BankName}&amount={amount}&des={encodedDes}",
    //         CreatedAt = c.CreatedAt,
    //         UpdatedAt = c.UpdatedAt
    //     });
    //     return result.AsQueryable();
    // }


    public IQueryable<BankAccountResponse> GetDefaultByUserId(Guid userId)
    {
        var bankAccount = _bankAccountRepository.GetDefaultByUserId(userId);
        // foreach (var c in bankAccount)
        // {
        //  c.ImageQR = $"https://qr.sepay.vn/img?acc={c.AccountNumber}&bank={c.BankName}";   
        // }
        return bankAccount.ProjectTo<BankAccountResponse>(_mapper.ConfigurationProvider);
    }
    public async Task<ApiResponse<BankAccountResponse>> AddBankAccount(Guid userId, CreateBankAccount request)
    {
        var encodedDes = Uri.EscapeDataString(request.Description ?? "");
     //   var amount = account.DefaultAmount.HasValue ? $"&amount={(int)account.DefaultAmount.Value}" : "";
        var qrUrl = $"https://qr.sepay.vn/img?acc={request.AccountNumber}&bank={request.BankName}&des={encodedDes}";
     
        var result = _mapper.Map<BankAccount>(request);
        result.ImageQR = qrUrl;
        result.UserId = userId;
        result.CreatedAt = DateTime.UtcNow;
       
        await _bankAccountRepository.Add(result);
      
        return ApiResponse<BankAccountResponse>.Success(_mapper.Map<BankAccountResponse>(result), "Bank Account created");
    }
    public async Task<ApiResponse<bool>> UpdateBankAccount(Guid id, UpdateBankAccount request)
    {
        var bankAccount = await _bankAccountRepository.GetById(id);
        if (bankAccount == null)
            throw new KeyNotFoundException("BankAccount not found");
        
        Console.WriteLine($"🔄 Updating bank account {id}:");
        Console.WriteLine($"   - Request IsActive: {request.IsActive}");
        Console.WriteLine($"   - Current IsActive: {bankAccount.IsActive}");
            
        var encodedDes = Uri.EscapeDataString(request.Description ?? "");
        //   var amount = account.DefaultAmount.HasValue ? $"&amount={(int)account.DefaultAmount.Value}" : "";
        var qrUrl = $"https://qr.sepay.vn/img?acc={request.AccountNumber}&bank={request.BankName}&des={encodedDes}"; 
        _mapper.Map(request, bankAccount);
        bankAccount.ImageQR = qrUrl;
        bankAccount.UpdatedAt = DateTime.UtcNow;
        
        Console.WriteLine($"   - After mapping IsActive: {bankAccount.IsActive}");
        
        await _bankAccountRepository.Update(bankAccount);
      
        return ApiResponse<bool>.Success(true, "Update Bank Account");
    }

    public async Task<ApiResponse<bool>> DeleteBankAccount(Guid id)
    {
        var bankAccount =await _bankAccountRepository.GetById(id);
        if (bankAccount == null)
            throw new KeyNotFoundException("BankAccount not found");
        await _bankAccountRepository.Delete(bankAccount);
        return ApiResponse<bool>.Success(true, "Delete Bank Account ");
    }

    public async Task<JsonElement?> GetTransactionsAsync()
    {
        var req = new HttpRequestMessage(HttpMethod.Get, "https://my.sepay.vn/userapi/transactions/list");
        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _settings.SecretKey);

        var resp = await _http.SendAsync(req);
        if (!resp.IsSuccessStatusCode)
        {
            var err = await resp.Content.ReadAsStringAsync();
            throw new Exception($"SePay API lỗi: {resp.StatusCode} - {err}");
        }

        var json = await resp.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<JsonElement>(json);
    }
    
  
}