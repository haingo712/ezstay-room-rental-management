using AutoMapper;
using HouseLocationAPI.DTO.Request;
using HouseLocationAPI.Models;

namespace HouseLocationAPI.Profiles
{
    public class HouseLocationProfile : Profile
    {
        public HouseLocationProfile()
        {
            CreateMap<HouseLocation, HouseLocationDTO>();
            CreateMap<CreateHouseLocationDTO, HouseLocation>();
            CreateMap<UpdateHouseLocationDTO, HouseLocation>();
        }
    }
}
