using AutoMapper;
using UtilityBillAPI.DTO.Request;
using UtilityBillAPI.Models;

namespace UtilityBillAPI.Profiles
{
    public class BillSettingProfile : Profile
    {
        public BillSettingProfile()
        {
            CreateMap<BillSetting, BillSettingDTO>();
            CreateMap<UpdateBillSettingDTO, BillSetting>();            
        }
    }
}
