using AutoMapper;
using eGranjaCAT.Application.DTOs.User;


namespace eGranjaCAT.Infrastructure.Identity
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<CreateUserDTO, User>()
                .ForMember(d => d.UserName, opt => opt.MapFrom(src => $"{src.Name}.{src.Lastname}"))
                .ForMember(d => d.Name, src => src.MapFrom(s => s.Name))
                .ForMember(d => d.Lastname, src => src.MapFrom(s => s.Lastname))
                .ForMember(d => d.Email, src => src.MapFrom(s => s.Email))
                .ForMember(d => d.Role, src => src.MapFrom(s => s.Role));

            CreateMap<User, TokenUserDTO>()
                .ForMember(d => d.Id, src => src.MapFrom(s => s.Id))
                .ForMember(d => d.Email, src => src.MapFrom(s => s.Email))
                .ForMember(d => d.Role, opt => opt.Ignore())
                .ForMember(d => d.Name, src => src.MapFrom(s => s.Name))
                .ForMember(d => d.Lastname, src => src.MapFrom(s => s.Lastname));

            CreateMap<User, GetUserDTO>()
                .ForMember(d => d.Id, src => src.MapFrom(s => s.Id))
                .ForMember(d => d.Email, src => src.MapFrom(s => s.Email))
                .ForMember(d => d.Role, opt => opt.Ignore());
        }
    }
}
