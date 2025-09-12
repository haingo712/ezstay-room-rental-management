using AutoMapper;
using BoardingHouseAPI.DTO.Request;
using BoardingHouseAPI.Models;

namespace BoardingHouseAPI.Profiles
{
    public class BoardingHouseProfile : Profile
    {
        public BoardingHouseProfile()
        {            
            CreateMap<BoardingHouse, BoardingHouseDTO>()
                 .ForMember(d => d.Location, opt => opt.MapFrom(s => s.Location));              
            CreateMap<CreateBoardingHouseDTO, BoardingHouse>();                        
            CreateMap<UpdateBoardingHouseDTO, BoardingHouse>();

            CreateMap<HouseLocation, HouseLocationDTO>();
            CreateMap<CreateHouseLocationDTO, HouseLocation>();
            CreateMap<UpdateHouseLocationDTO, HouseLocation>();
        }
    }
}
