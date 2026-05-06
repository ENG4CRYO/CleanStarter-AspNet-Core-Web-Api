using CleanStarter.Application.Validators.Auth;
using FluentValidation;


namespace CleanStarter.Application.Features.Auth.Commands.ResetPassword
{
    public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordCommandValidator()
        {
            RuleFor(x => x.ResetToken).NotEmpty().WithMessage("Reset token is required.");
            RuleFor(x => x.OtpCode).NotEmpty().Length(6).WithMessage("Valid OTP is required.");
            RuleFor(x => x.NewPassword).NotEmpty().MinimumLength(6).WithMessage("Password must be at least 6 characters.");
        }
    }
}
