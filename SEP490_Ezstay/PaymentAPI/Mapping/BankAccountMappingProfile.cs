using AutoMapper;
using PaymentAPI.DTOs.Requests;
using PaymentAPI.Model;
using Shared.DTOs.Payments.Responses;

namespace PaymentAPI.Mapping;

public class BankAccountMappingProfile:Profile
{
    public BankAccountMappingProfile()
    {
        CreateMap<CreateBankAccount, BankAccount>();
        CreateMap<UpdateBankAccount, BankAccount>();
        CreateMap<BankAccount, BankAccountResponse>();
        CreateMap<BankGateway, BankGatewayResponse>();

    }
}