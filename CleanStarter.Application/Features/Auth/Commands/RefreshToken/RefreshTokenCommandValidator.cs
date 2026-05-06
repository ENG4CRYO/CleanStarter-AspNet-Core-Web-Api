using CleanStarter.Application.Validators.Auth;
using FluentValidation;

namespace CleanStarter.Application.Features.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenCommandValidator()
        {
            RuleFor(x => x.Token).NotNull().WithMessage("Refresh Token Cannot Be Null")
            .NotEmpty().WithMessage("Refresh Token Canneot Be Empty");
        }
    }
}