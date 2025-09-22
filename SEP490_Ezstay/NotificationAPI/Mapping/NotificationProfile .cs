using AutoMapper;
using NotificationAPI.DTOs.Respone;
using NotificationAPI.DTOs.Resquest;
using NotificationAPI.Model;

namespace NotificationAPI.Mapping
{
    public class NotificationProfile : Profile
    {
        public NotificationProfile()
        {
            CreateMap<CreateNotificationRequestDto, Notify>();
            CreateMap<Notify, NotificationResponseDto>();
        }
    }
}
