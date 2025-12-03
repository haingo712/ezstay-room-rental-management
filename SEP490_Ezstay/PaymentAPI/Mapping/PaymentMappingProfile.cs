using AutoMapper;
using PaymentAPI.DTOs.Responses;
using PaymentAPI.Model;

namespace PaymentAPI.Mapping;

public class PaymentMappingProfile : Profile
{
    public PaymentMappingProfile()
    {
        CreateMap<Payment, PaymentResponse>();
    }
}
