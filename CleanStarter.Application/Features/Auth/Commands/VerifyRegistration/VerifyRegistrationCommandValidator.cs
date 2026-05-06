using CleanStarter.Application.Dtos.AuthModel;
using CleanStarter.Application.Validators.Auth;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanStarter.Application.Features.Auth.Commands.VerifyRegistration
{
    public class VerifyRegistrationCommandValidator : AbstractValidator<VerifyRegistrationCommand>
    {
        public VerifyRegistrationCommandValidator()
        {
            RuleFor(x => x.RegisterToken)
                .NotEmpty().WithMessage("Registration token is required.");

            RuleFor(x => x.OtpCode)
                .NotEmpty().WithMessage("OTP code is required.")
                .Length(6).WithMessage("OTP code must be exactly 6 digits.");
        }
    }
}
