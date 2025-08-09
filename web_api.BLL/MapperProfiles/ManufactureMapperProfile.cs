using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using web_api.BLL.DTOs.Manufactures;
using web_api.DAL.Entities;

namespace web_api.BLL.MapperProfiles
{
    public class ManufactureMapperProfile : Profile
    {
        public ManufactureMapperProfile()
        {
            // ManufactureDto <-> Manufacture
            CreateMap<ManufactureDto, Manufacture>().ReverseMap();

            // ManufactureCreateDto -> Manufacture
            CreateMap<ManufactureCreateDto, Manufacture>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
                .ForMember(dest => dest.Image, opt => opt.Ignore())
                .ForMember(dest => dest.Cars, opt => opt.Ignore());

            // ManufactureUpdateDto -> Manufacture
            CreateMap<ManufactureUpdateDto, Manufacture>()
                .ForMember(dest => dest.Image, opt => opt.Ignore())
                .ForMember(dest => dest.Cars, opt => opt.Ignore());
        }
    }
}
