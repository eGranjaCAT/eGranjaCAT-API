using AutoMapper;
using eGranjaCAT.Application.DTOs.Entrada;
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
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            CreateMap<Entrada, GetEntradaDTO>()
                .ForMember(dest => dest.Categoria, opt => opt.MapFrom(src => src.Categories))
                .ForMember(dest => dest.Origen, opt => opt.MapFrom(src => src.Origen));
        }
    }
}
