using FluentValidation;

namespace CleanStarter.Application.Features.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenCommandValidator()
        {
            RuleFor(x => x.Model).SetValidator(new Validators.RequestRefreshTokenValidator());
        }
    }
}