using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace web_api.BLL.DTOs.Account
{
    public class LoginDto
    {
        public string? Login { get; set; } = string.Empty;
        public string? Password { get; set; } = string.Empty;
    }

    public class LoginValidator : AbstractValidator<LoginDto>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Login)
                .NotEmpty().WithMessage("Login is required");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("minimum length 6 characters");
        }
    }
}