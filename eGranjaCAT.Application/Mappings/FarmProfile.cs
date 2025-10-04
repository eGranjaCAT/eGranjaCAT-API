using AutoMapper;
using eGranjaCAT.Application.DTOs.Farm;
using eGranjaCAT.Application.DTOs.User;
using eGranjaCAT.Application.Entities;


namespace eGranjaCAT.Application.Mappings
{
    public class FarmProfile : Profile
    {
        public FarmProfile()
        {
            CreateMap<CreateFarmDTO, Farm>();


            CreateMap<Farm, GetFarmDTO>()
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