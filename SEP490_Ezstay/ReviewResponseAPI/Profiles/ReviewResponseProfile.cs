using AutoMapper;
using ReviewResponseAPI.DTO.Requests;
using ReviewResponseAPI.Model;

namespace ReviewResponseAPI.Profiles;

public class ReviewResponseProfile : Profile
{
    public ReviewResponseProfile()
    {
        CreateMap<CreateReviewResponseDto, ReviewResponse>();
        CreateMap<UpdateReviewResponseDto, ReviewResponse>();
        CreateMap<ReviewResponse, ReviewResponseDto>();
    }
}