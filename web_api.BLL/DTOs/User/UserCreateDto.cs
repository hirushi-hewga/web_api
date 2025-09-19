using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using web_api.BLL.DTOs.Role;

namespace web_api.BLL.DTOs.User
{
    public class UserCreateDto
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool EmailConfirmed { get; set; } = false;
        public IFormFile? Image { get; set; }
        public IEnumerable<string> Roles { get; set; } = [];
    }

    public class UserCreateValidator : AbstractValidator<UserCreateDto>
    {
        public UserCreateValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username is required")
                .MaximumLength(20).WithMessage("maximum length 20 characters");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Incorrect email format");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("minimum length 6 characters");

            RuleFor(x => x.FirstName)
                .MaximumLength(20).WithMessage("maximum length 20 characters");

            RuleFor(x => x.LastName)
                .MaximumLength(20).WithMessage("maximum length 20 characters");
        }
    }
}
