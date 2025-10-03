using AutoMapper;
using ReviewAPI.DTO.Requests.ReviewReply;
using ReviewAPI.DTO.Response.ReviewReply;
using ReviewAPI.Model;

namespace ReviewAPI.Profiles;

public class ReviewReplyProfile: Profile
{
    public ReviewReplyProfile()
    {
        CreateMap<CreateReviewReplyRequest, ReviewReply>();
        CreateMap<UpdateReviewReplyRequest, ReviewReply>();
        CreateMap<ReviewReply, ReviewReplyResponse>();
    }
}