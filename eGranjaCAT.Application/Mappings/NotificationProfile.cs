using AutoMapper;
using eGranjaCAT.Application.DTOs.Notification;
using eGranjaCAT.Application.DTOs.User;
using eGranjaCAT.Domain.Entities;


namespace eGranjaCAT.Application.Mappings
{
    public class NotificationProfile : Profile
    {
        public NotificationProfile()
        {
            CreateMap<Notification, GetNotificationDTO>()
                .ForMember(dest => dest.User, opt => opt.MapFrom((src, dest, destMember, context) =>
                {
                    if (context.Items.TryGetValue("UserDict", out var dictObj) && dictObj is Dictionary<string, GetUserDTO> dict)
                    {
                        var key = src.UserGuid?.ToString();
                        if (!string.IsNullOrEmpty(key) && dict.TryGetValue(key, out var userDto)) return userDto;
                    }
                    return null;
                }));
        }
    }
}
