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
            // Car -> CarDto
            CreateMap<Car, CarDto>()
                .ForMember(dest => dest.Manufacture, opt => opt.MapFrom(src => src.Manufacture == null ? "" : src.Manufacture.Name))
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images.Select(i => Path.Combine(i.Path, i.Name))));

            CreateMap<CarCreateDto, Car>()
                .ForMember(dest => dest.Images, opt => opt.Ignore())
                .ForMember(dest => dest.Manufacture, opt => opt.Ignore());

            CreateMap<CarUpdateDto, Car>()
                .ForMember(dest => dest.Images, opt => opt.Ignore())
                .ForMember(dest => dest.Manufacture, opt => opt.Ignore());
        }
    }
}
