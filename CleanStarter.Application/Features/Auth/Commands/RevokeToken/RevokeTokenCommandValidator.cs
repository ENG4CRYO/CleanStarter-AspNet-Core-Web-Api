using FluentValidation;
using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanStarter.Application.Features.Auth.Commands.RevokeToken
{
    public class RevokeTokenCommandValidator : AbstractValidator<RevokeTokenCommand>    
    {
        public RevokeTokenCommandValidator()
        {
            RuleFor(x => x.Token).NotNull().WithMessage("Token Request Cannot Be Null")
            .NotEmpty().WithMessage("Token Request Canneot Be Empty");
        }
    }
}
