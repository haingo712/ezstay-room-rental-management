using AutoMapper;
using FavoritePostAPI.DTO.Request;
using FavoritePostAPI.Models;

namespace FavoritePostAPI.Profiles
{
    public class FavoritePostProfile : Profile
    {
        public FavoritePostProfile()
        {
            CreateMap<FavoritePost, FavoritePostDTO>();
            CreateMap<FavoritePostCreateDTO, FavoritePost>()
              .ForMember(dest => dest.Id, opt => opt.Ignore())
              .ForMember(dest => dest.AccountId, opt => opt.Ignore())
              .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
        }
    }
}
