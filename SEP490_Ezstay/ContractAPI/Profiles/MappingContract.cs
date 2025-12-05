using AutoMapper;
using ContractAPI.DTO.Requests;
using ContractAPI.DTO.Requests.ServiceInfor;
using ContractAPI.DTO.Response;
using ContractAPI.Model;
using Shared.DTOs.Contracts.Responses;
using Shared.DTOs.ServiceInfors.Response;

namespace ContractAPI.Profiles;

public class MappingContract:Profile
{
    public MappingContract()
    {
        CreateMap<CreateContract, Contract>();
        CreateMap<UpdateContract, Contract>();
        CreateMap<Contract, ContractResponse>();
        CreateMap<ExtendContract, Contract>();
      
        CreateMap<CreateService, ServiceInfor>();
        CreateMap<UpdateService, ServiceInfor>(); 
        CreateMap<ServiceInfor, ServiceInforResponse>();
        
        CreateMap<CreateRentalRequest, RentalRequest>();
        CreateMap<RentalRequest, RentalRequestResponse>();
    }
    
}