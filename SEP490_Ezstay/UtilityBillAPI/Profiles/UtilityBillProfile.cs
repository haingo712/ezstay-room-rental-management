using AutoMapper;
using UtilityBillAPI.DTO.Request;
using UtilityBillAPI.Models;

namespace UtilityBillAPI.Profiles
{
    public class UtilityBillProfile : Profile
    {
        public UtilityBillProfile()
        {
            CreateMap<UtilityBill, UtilityBillDTO>();
            CreateMap<UtilityBillDTO, UtilityBill>();
            CreateMap<UpdateUtilityBillDTO, UtilityBill>();
        }
    }
}
