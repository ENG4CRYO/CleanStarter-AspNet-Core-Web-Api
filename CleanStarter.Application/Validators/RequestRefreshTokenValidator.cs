using FluentValidation;
using CleanStarter.Application.Dtos.AuthModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanStarter.Application.Validators
{
    public class RequestRefreshTokenValidator : AbstractValidator<RequestRefreshToken>
    {
        public RequestRefreshTokenValidator()
        {
            RuleFor(x => x.Token).NotNull().WithMessage("Refresh Token Cannot Be Null")
                .NotEmpty().WithMessage("Refresh Token Canneot Be Empty");
        }
    }
}
