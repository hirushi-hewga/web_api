using AutoMapper;
using web_api.BLL.DTOs.Account;
using web_api.BLL.DTOs.User;
using web_api.DAL.Entities;

namespace web_api.BLL.MapperProfiles
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile()
        {
            // RegisterDto -> AppUser
            CreateMap<RegisterDto, AppUser>();
            
            // UserDto <-> AppUser
            CreateMap<AppUser, UserDto>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles));
            
            
        }
    }
}