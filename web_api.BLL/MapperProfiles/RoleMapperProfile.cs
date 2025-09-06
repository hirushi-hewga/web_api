using AutoMapper;
using web_api.BLL.DTOs.Role;
using web_api.DAL.Entities;

namespace web_api.BLL.MapperProfiles
{
    public class RoleMapperProfile : Profile
    {
        public RoleMapperProfile()
        {
            // AppRole <-> RoleDto
            CreateMap<AppRole, RoleDto>().ReverseMap();

            // AppUserRole -> RoleDto
            CreateMap<AppUserRole, RoleDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.RoleId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Role == null ? "" : src.Role.Name));
        }
    }
}