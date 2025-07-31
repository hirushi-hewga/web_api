using FluentValidation;
using web_api.BLL.DTOs.Role;

namespace web_api.BLL.DTOs.User
{
    public class UserDto
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public IEnumerable<RoleDto> Roles { get; set; } = [];
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
                .NotEmpty().WithMessage("First name is required")
                .MaximumLength(20).WithMessage("maximum length 20 characters");
            
            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .MaximumLength(20).WithMessage("maximum length 20 characters");
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