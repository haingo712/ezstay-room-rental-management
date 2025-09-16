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
            CreateMap<RentalpostDTO, RentalPosts>().ReverseMap()
                 .ForMember(dest => dest.AuthorName, opt => opt.Ignore())
                 .ForMember(dest => dest.RoomName, opt => opt.Ignore())
                 .ForMember(dest => dest.HouseName, opt => opt.Ignore());
            CreateMap<UpdateRentalPostDTO,RentalPosts>();
        }

    }
}
