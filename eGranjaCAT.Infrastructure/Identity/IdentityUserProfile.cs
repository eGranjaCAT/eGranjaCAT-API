using AutoMapper;
using eGranjaCAT.Application.Common;
using eGranjaCAT.Application.DTOs.User;


namespace eGranjaCAT.Infrastructure.Identity
{
    public class IdentityUserProfile : Profile
    {
        public IdentityUserProfile()
        {
            CreateMap<UserBaseModel, User>()
                .ForMember(d => d.UserName, opt => opt.MapFrom(s => s.Email))
                .ForMember(d => d.Id, opt => opt.Ignore());

            CreateMap<User, TokenUserDTO>();

            CreateMap<User, GetUserDTO>();
        }
    }
}
