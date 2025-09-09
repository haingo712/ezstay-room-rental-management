using AutoMapper;
using ReviewAPI.DTO.Requests;
using ReviewAPI.Model;

namespace ReviewAPI.Profiles;

public class ReviewProfile : Profile
{
    public ReviewProfile()
    {
        CreateMap<CreateReviewDto, Review>();
        CreateMap<UpdateReviewDto, Review>();
        CreateMap<Review, ReviewDto>();
    }
}