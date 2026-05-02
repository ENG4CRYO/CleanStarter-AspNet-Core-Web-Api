using CleanStarter.Application.Validators;
using FluentValidation;

namespace CleanStarter.Application.Features.Auth.Commands.ForgotPassword
{
    public class ForgotPasswordCommandValidator : AbstractValidator<ForgotPasswordCommand>
    {
        public ForgotPasswordCommandValidator()
        {
            RuleFor(x => x.Model).SetValidator(new ForgotPasswordValidator());
        }
    }
}
