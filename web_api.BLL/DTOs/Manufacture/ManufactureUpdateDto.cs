using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using web_api.DAL.Entities;

namespace web_api.BLL.DTOs.Manufactures
{
    public class ManufactureUpdateDto
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public string? Founder { get; set; } = string.Empty;
        public string? Director { get; set; } = string.Empty;
        public IFormFile? Image { get; set; }
        public IEnumerable<Car>? Cars { get; set; }
    }

    public class ManufactureUpdateValidator : AbstractValidator<ManufactureUpdateDto>
    {
        public ManufactureUpdateValidator()
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
