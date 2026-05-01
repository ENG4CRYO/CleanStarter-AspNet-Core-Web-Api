using CleanStarter.Application.Validators;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanStarter.Application.Features.Auth.Commands.InitiateRegistration
{
    public class InitiateRegistrationCommandValidator : AbstractValidator<InitiateRegistrationCommand>
    {
        public InitiateRegistrationCommandValidator()
        {
            RuleFor(x => x.Model).SetValidator(new InitiateRegistrationValidator());
        }
    }
}
