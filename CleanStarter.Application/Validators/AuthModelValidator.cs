using FluentValidation;
using CleanStarter.Application.Dtos.AuthModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanStarter.Application.Validators
{
    public class AuthModelValidator : AbstractValidator<AuthModel>
    {
        public AuthModelValidator()
        {
            RuleFor(x => x.UserName).
                NotEmpty().WithMessage("User Name Is Required")
                .Length(4, 10).WithMessage("The User Name Length Must Be Between 3 And 10");

            RuleFor(x => x.Email).
                NotEmpty().WithMessage("Email Is Required")
                .EmailAddress().WithMessage("Invalid Email Format");

        }
    }
}
