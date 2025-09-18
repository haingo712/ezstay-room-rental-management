using AuthApi.DTO.Request;
using AuthApi.DTO.Response;
using AuthApi.Models;
using AutoMapper;

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
