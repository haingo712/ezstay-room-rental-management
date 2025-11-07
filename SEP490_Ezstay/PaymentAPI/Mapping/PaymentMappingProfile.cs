using AutoMapper;
using PaymentAPI.DTOs.Responses;
using PaymentAPI.Model;

namespace PaymentAPI.Mapping;

public class PaymentMappingProfile : Profile
{
    public PaymentMappingProfile()
    {
        // Payment -> PaymentDetailResponse
        CreateMap<Payment, PaymentDetailResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
            .ForMember(dest => dest.UtilityBillId, opt => opt.MapFrom(src => src.UtilityBillId.ToString()))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.TenantId.ToString()))
            .ForMember(dest => dest.TenantId, opt => opt.MapFrom(src => src.TenantId.ToString()))
            .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod.ToString()))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.ApprovedBy, opt => opt.MapFrom(src => src.ApprovedBy.HasValue ? src.ApprovedBy.Value.ToString() : null))
            .ForMember(dest => dest.ApprovedDate, opt => opt.MapFrom(src => src.ApprovedAt))
            .ForMember(dest => dest.RejectReason, opt => opt.MapFrom(src => src.RejectionReason));

        // Payment -> PaymentInfo
        CreateMap<Payment, PaymentInfo>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
            .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod.ToString()))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
    }
}
