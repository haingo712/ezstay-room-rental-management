using AuthApi.DTO.Request;
using AuthApi.DTO.Response;
using AuthApi.Models;
using AutoMapper;
using Shared.DTOs.Auths.Responses;

namespace AuthApi.Mapping
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<AccountRequest, Account>();
            CreateMap<Account, AccountResponse>();
        }
    }
}
