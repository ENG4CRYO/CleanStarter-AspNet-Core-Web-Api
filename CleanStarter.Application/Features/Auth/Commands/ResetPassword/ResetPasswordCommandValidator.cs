using CleanStarter.Application.Validators;
using FluentValidation;


namespace CleanStarter.Application.Features.Auth.Commands.ResetPassword
{
    public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordCommandValidator()
        {
            RuleFor(x => x.Model).SetValidator(new ResetPasswordValidator());
        }
    }
}
