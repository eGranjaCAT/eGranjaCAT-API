using AutoMapper;
using eGranjaCAT.Application.DTOs.Lot;
using eGranjaCAT.Application.DTOs.User;
using eGranjaCAT.Domain.Entities;


namespace eGranjaCAT.Application.Mappings
{
    public class LotProfile : Profile
    {
        public LotProfile()
        {
            CreateMap<CreateLotDTO, Lot>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Updated, opt => opt.Ignore())
                .ForMember(dest => dest.Active, opt => opt.MapFrom(src => true));

            CreateMap<UpdateLotDTO, Lot>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.FarmId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UserGuid, opt => opt.Ignore())
                .ForMember(dest => dest.Farm, opt => opt.Ignore());

            CreateMap<Lot, GetLotDTO>()
                .ForMember(dest => dest.Farm, opt => opt.MapFrom(src => src.Farm))
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

            CreateMap<Lot, GetLotNoRelationsDTO>();
        }
    }
}