using AutoMapper;
using ReviewReportAPI.DTO.Requests;
using ReviewReportAPI.DTO.Response;
using ReviewReportAPI.Model;

namespace ReviewReportAPI.Profiles;

public class ReviewReportProfile : Profile
{
    public ReviewReportProfile()
    {
        CreateMap<CreateReviewReportRequest, ReviewReport>();
        CreateMap<UpdateReviewReportDto, ReviewReport>();
        CreateMap<ReviewReport, ReviewReportResponse>();
    }
}