using AutoMapper;
using ContractAPI.DTO.Requests;
using ContractAPI.DTO.Response;
using ContractAPI.Model;
using Shared.DTOs.Contracts.Responses;

namespace ContractAPI.Profiles;

public class MappingContract:Profile
{
    public MappingContract()
    {
        CreateMap<CreateContract, Contract>();
        CreateMap<UpdateContract, Contract>();
        CreateMap<Contract, ContractResponse>();
        CreateMap<ExtendContractDto, Contract>();
      
    }
    
}