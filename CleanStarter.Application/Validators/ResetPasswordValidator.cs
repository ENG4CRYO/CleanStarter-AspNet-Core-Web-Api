using CleanStarter.Application.Dtos.AuthModel;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanStarter.Application.Validators
{
    public class ResetPasswordValidator : AbstractValidator<ResetPasswordRequest>
    {
        public ResetPasswordValidator()
        {
            RuleFor(x => x.ResetToken).NotEmpty().WithMessage("Reset token is required.");
            RuleFor(x => x.OtpCode).NotEmpty().Length(6).WithMessage("Valid OTP is required.");
            RuleFor(x => x.NewPassword).NotEmpty().MinimumLength(6).WithMessage("Password must be at least 6 characters.");
        }
    }
}
