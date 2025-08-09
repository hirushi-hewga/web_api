using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using web_api.DAL.Entities;

namespace web_api.BLL.DTOs.Manufactures
{
    public class ManufactureDto
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Founder { get; set; }
        public string? Director { get; set; }
        public string? Image { get; set; }
    }
}
