using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using web_api.BLL.DTOs.Manufactures;

namespace web_api.BLL.DTOs.Cars
{
    public class CarDto
    {
        public string Id { get; set; }
        public string? Model { get; set; }
        public string? Brand { get; set; }
        public int Year { get; set; }
        public decimal? Price { get; set; }
        public string? Gearbox { get; set; }
        public string? Color { get; set; }
        public IEnumerable<string>? Images { get; set; }
        public ManufactureDto? Manufacture { get; set; }
    }
}
