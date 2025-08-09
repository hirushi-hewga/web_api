using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using web_api.BLL.DTOs.Cars;
using web_api.DAL.Entities;

namespace web_api.BLL.MapperProfiles
{
    public class CarMapperProfile : Profile
    {
        public CarMapperProfile()
        {
            // CarDto <-> Car
            CreateMap<CarDto, Car>().ReverseMap();

            CreateMap<CarCreateDto, Car>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
                .ForMember(dest => dest.Images, opt => opt.Ignore())
                .ForMember(dest => dest.Manufacture, opt => opt.Ignore());

            CreateMap<CarUpdateDto, Car>()
                .ForMember(dest => dest.Images, opt => opt.Ignore())
                .ForMember(dest => dest.Manufacture, opt => opt.Ignore());
        }
    }
}
