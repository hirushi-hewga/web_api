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
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public string? Founder { get; set; } = string.Empty;
        public string? Director { get; set; } = string.Empty;
    }

    public class ManufactureValidator : AbstractValidator<ManufactureDto>
    {
        public ManufactureValidator() 
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(255).WithMessage("Maximum length 255 characters");
            RuleFor(x => x.Description)
                .MaximumLength(10000).WithMessage("Maximum length 10000 characters");
            RuleFor(x => x.Founder)
                .NotEmpty().WithMessage("Founder is required")
                .MaximumLength(255).WithMessage("Maximium length 255 characters");
            RuleFor(x => x.Director)
                .NotEmpty().WithMessage("Director is required")
                .MaximumLength(255).WithMessage("Maximum length 255 characters");
        }
    }
}
