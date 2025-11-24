using AutoMapper;
using ReviewAPI.DTO.Requests;
using ReviewAPI.DTO.Response;
using ReviewAPI.Model;
using Shared.DTOs.Reviews.Responses;

namespace ReviewAPI.Profiles;

public class ReviewProfile : Profile
{
    public ReviewProfile()
    {
        CreateMap<CreateReviewRequest, Review>();
        CreateMap<UpdateReviewRequest, Review>();
        CreateMap<Review, ReviewResponse>();
    }
}