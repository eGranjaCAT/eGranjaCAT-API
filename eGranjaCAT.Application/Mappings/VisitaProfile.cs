using AutoMapper;
using eGranjaCAT.Application.DTOs.User;
using eGranjaCAT.Application.DTOs.Visites;
using eGranjaCAT.Domain.Entities;


namespace eGranjaCAT.Application.Mappings
{
    public class VisitaProfile : Profile
    {
        public VisitaProfile()
        {
            CreateMap<CreateVisitaDTO, Visita>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Updated, opt => opt.Ignore());

            CreateMap<Visita, GetVisitaDTO>()
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