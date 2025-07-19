using FluentValidation;

namespace web_api.BLL.DTOs.User
{
    public class UserDto
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    public class UserCreateDto : UserDto
    {
        public string Password { get; set; } = string.Empty;
    }

    public class UserUpdateDto : UserCreateDto
    {
        public string NewPassword { get; set; } = string.Empty;
    }
    
    public class UserCreateValidator : AbstractValidator<UserCreateDto>
    {
        public UserCreateValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required")
                .MaximumLength(20).WithMessage("maximum length 20 characters");
            
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Incorrect email format");
            
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("minimum length 6 characters");
        }
    }

    public class UserUpdateValidator : AbstractValidator<UserUpdateDto>
    {
        public UserUpdateValidator()
        {
            Include(new UserCreateValidator());
            
            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("New password is required")
                .MinimumLength(6).WithMessage("minimum length 6 characters");;
        }
    }
}