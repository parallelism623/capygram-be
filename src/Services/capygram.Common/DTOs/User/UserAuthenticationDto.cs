using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace capygram.Common.DTOs.User
{
    public class UserAuthenticationDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }

    }
    public class UserAuthenticationDtoValidation : AbstractValidator<UserAuthenticationDto>
    {
        public UserAuthenticationDtoValidation()
        {
            RuleFor(x => x.UserName).NotEmpty()
                 .WithMessage("Username cannot be empty.")
                 .Matches("^[a-zA-Z0-9]*$").WithMessage("Username can only contain letters and numbers.");
            RuleFor(user => user.Password)
                .NotEmpty().WithMessage("Password cannot be empty.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .MaximumLength(16).WithMessage("Password must be at most 16 characters long.");
        }
    }
}
