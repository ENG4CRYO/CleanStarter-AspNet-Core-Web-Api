using CleanStarter.Application.Validators.Auth;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanStarter.Application.Features.Auth.Commands.Login
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email Is Required")
                .EmailAddress().WithMessage("Invalid Email Format");

                 RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password Is Required");
        }
    }
}
