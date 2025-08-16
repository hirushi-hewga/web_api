using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using web_api.BLL.DTOs.Manufactures;

namespace web_api.BLL.DTOs.Cars
{
    public class CarUpdateDto
    {
        public string Id { get; set; }
        public string? Model { get; set; }
        public string? Brand { get; set; }
        public int Year { get; set; } = 0;
        public decimal? Price { get; set; }
        public string? Gearbox { get; set; }
        public string? Color { get; set; }
        public string? Manufacture { get; set; }
        public IEnumerable<IFormFile>? Images { get; set; } = [];
    }

    public class CarUpdateValidator : AbstractValidator<CarUpdateDto>
    {
        public CarUpdateValidator()
        {
            RuleFor(x => x.Model)
                .NotEmpty().WithMessage("Model is required")
                .MaximumLength(100).WithMessage("Maximum length 100 characters");
            RuleFor(x => x.Brand)
                .NotEmpty().WithMessage("Brand is required")
                .MaximumLength(100).WithMessage("Maximum length 100 characters");
            RuleFor(x => x.Year)
                .InclusiveBetween(1800, int.MaxValue)
                .WithMessage("Year must be no earlier than 1800.");
            RuleFor(x => x.Price)
                .NotNull().WithMessage("Price is required")
                .GreaterThanOrEqualTo(0).WithMessage("Price must be a positive number");
            RuleFor(x => x.Gearbox)
                .MaximumLength(50).WithMessage("Maximum length 50 characters");
            RuleFor(x => x.Color)
                .MaximumLength(50).WithMessage("Maximum length 50 characters");
        }
    }
}
