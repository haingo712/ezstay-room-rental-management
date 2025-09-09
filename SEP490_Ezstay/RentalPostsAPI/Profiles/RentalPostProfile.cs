using AutoMapper;
using RentalPostsAPI.DTO.Request;
using RentalPostsAPI.Models;

namespace RentalPostsAPI.Profiles
{
    public class RentalPostProfile : Profile
    {
        public RentalPostProfile()
        {
            CreateMap<CreateRentalPostDTO, RentalPosts>();
            CreateMap<RentalpostDTO, RentalPosts>().ReverseMap();
            CreateMap<UpdateRentalPostDTO,RentalPosts>();
        }

    }
}
