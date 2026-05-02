using CleanStarter.Application.Dtos.AuthModel;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanStarter.Application.Validators
{
    public class ForgotPasswordValidator : AbstractValidator<ForgotPasswordRequest> 
    {
        public ForgotPasswordValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
        }
    }
}
