using AutoMapper;
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

            CreateMap<Visita, GetVisitaDTO>();
        }
    }
}