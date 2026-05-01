using FluentValidation;
using CleanStarter.Application.Dtos.AuthModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanStarter.Application.Validators
{
    public class TokenRequestModelValidator : AbstractValidator<TokenRequestModel>
    {
        public TokenRequestModelValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email Is Required")
                .EmailAddress().WithMessage("Invalid Email Format");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password Is Required");
               
        }
    }
}
