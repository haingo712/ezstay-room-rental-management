using AutoMapper;
using ContractAPI.DTO.Requests;
using ContractAPI.DTO.Response;
using ContractAPI.Model;

namespace ContractAPI.Profiles;

public class MappingContract:Profile
{
    public MappingContract()
    {
        CreateMap<CreateContractDto, Contract>();
        CreateMap<UpdateContractDto, Contract>();
        CreateMap<Contract, ContractResponseDto>();
        CreateMap<ExtendContractDto, Contract>();
      
    }
    
}