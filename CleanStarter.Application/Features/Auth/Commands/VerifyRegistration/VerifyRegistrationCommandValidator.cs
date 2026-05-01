using CleanStarter.Application.Dtos.AuthModel;
using CleanStarter.Application.Validators;
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
            RuleFor(x => x.Model).SetValidator(new VerifyRegistrationValidator());
        }
    }
}
