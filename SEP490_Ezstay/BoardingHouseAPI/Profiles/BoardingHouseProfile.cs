using AutoMapper;
using BoardingHouseAPI.DTO.Request;
using BoardingHouseAPI.Models;

namespace BoardingHouseAPI.Profiles
{
    public class BoardingHouseProfile : Profile
    {
        public BoardingHouseProfile()
        {            
            CreateMap<BoardingHouse, BoardingHouseDTO>();             
            
            CreateMap<CreateBoardingHouseDTO, BoardingHouse>();            
            
            CreateMap<UpdateBoardingHouseDTO, BoardingHouse>();            
        }
    }
}
