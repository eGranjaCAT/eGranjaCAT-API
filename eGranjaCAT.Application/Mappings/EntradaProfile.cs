using AutoMapper;
using eGranjaCAT.Application.DTOs.Entrada;
using eGranjaCAT.Application.DTOs.User;
using eGranjaCAT.Domain.Entities;


namespace eGranjaCAT.Application.Mappings
{
    public class EntradaProfile : Profile
    {
        public EntradaProfile()
        {
            CreateMap<CreateEntradaDTO, Entrada>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Updated, opt => opt.Ignore());

            CreateMap<Entrada, GetEntradaDTO>()
                .ForMember(dest => dest.Categoria, opt => opt.MapFrom(src => src.Categories))
                .ForMember(dest => dest.Origen, opt => opt.MapFrom(src => src.Origen))
                .ForMember(dest => dest.User, opt => opt.MapFrom((src, dest, destMember, context) =>
                {
                    if (context.Items.TryGetValue("UserDict", out var dictObj) && dictObj is Dictionary<string, GetUserDTO> dict)
                    {
                        var key = src.UserGuid?.ToString();
                        if (!string.IsNullOrEmpty(key) && dict.TryGetValue(key, out var userDto))
                            return userDto;
                    }
                    return null;
                }));
        }
    }
}
