using AutoMapper;
using eGranjaCAT.Application.DTOs.Farm;
using eGranjaCAT.Application.DTOs.User;
using eGranjaCAT.Domain.Entities;


namespace eGranjaCAT.Application.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<CreateUserDTO, UserBase>().ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

            CreateMap<UserBase, TokenUserDTO>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
            CreateMap<UserBase, GetUserDTO>();
        }
    }
}