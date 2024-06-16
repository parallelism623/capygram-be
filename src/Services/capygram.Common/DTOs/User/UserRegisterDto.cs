using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace capygram.Common.DTOs.User
{
    public class UserRegisterDto
    {
        public string UserName { get;set; }
        public string Password { get;set; } 
        public string Email { get; set; }
        public string FullName { get; set; } 
        public DateTime Birthday { get; set; }
        public string AvatarUrl { get; set; } = "https://i.pinimg.com/736x/bc/43/98/bc439871417621836a0eeea768d60944.jpg";
        public string OTP { get; set; }
    }
    public class UserRegisterDtoValidation : AbstractValidator<UserRegisterDto>
    {
        public UserRegisterDtoValidation()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username cannot be empty.")
                .Matches("^[a-zA-Z0-9]*$").WithMessage("Username can only contain letters and numbers.");
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is not allowed empty")
                .EmailAddress().WithMessage("Invalid email");
            RuleFor(user => user.Password)
                .NotEmpty().WithMessage("Password cannot be empty.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .MaximumLength(16).WithMessage("Password must be at most 16 characters long.");
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Name cannot be empty");
            RuleFor(x => x.Birthday)
                .NotEmpty().WithMessage("Birth day is required");
        }
    }
}
