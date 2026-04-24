using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanStarter.Application.Features.Auth.Queries.Login
{
#if IsCQRS
    public class LoginQueryValidator : AbstractValidator<LoginQuery>
    {
        public LoginQueryValidator()
        {
            RuleFor(x => x.Model).SetValidator(new Validators.TokenRequsetModelValidator());
        }
    }
#endif
}
