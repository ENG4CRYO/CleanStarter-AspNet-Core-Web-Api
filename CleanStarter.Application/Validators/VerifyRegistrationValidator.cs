using CleanStarter.Application.Dtos.AuthModel;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanStarter.Application.Validators
{
    public class VerifyRegistrationValidator : AbstractValidator<VerifyRegistrationRequest>
    {
        public VerifyRegistrationValidator()
        {
            RuleFor(x => x.RegisterToken)
                .NotEmpty().WithMessage("Registration token is required.");

            RuleFor(x => x.OtpCode)
                .NotEmpty().WithMessage("OTP code is required.")
                .Length(6).WithMessage("OTP code must be exactly 6 digits.");
        }
    }
}
