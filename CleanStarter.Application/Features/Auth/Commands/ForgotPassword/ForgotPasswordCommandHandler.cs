using CleanStarter.Application.Common;
using CleanStarter.Application.Dtos.AuthModel;
using CleanStarter.Application.Interfaces.Infrastructure;
using CleanStarter.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CleanStarter.Application.Features.Auth.Commands.ForgotPassword
{
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, ApiResponse<string>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICacheService _cacheService;
        private readonly IEmailService _emailService;

        public ForgotPasswordCommandHandler(
            UserManager<ApplicationUser> userManager,
            ICacheService cacheService,
            IEmailService emailService)
        {
            _userManager = userManager;
            _cacheService = cacheService;
            _emailService = emailService;
        }

        public async Task<ApiResponse<string>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Model.Email);

            var resetToken = Guid.NewGuid().ToString();

            if (user != null)
            {
                var otpCode = new Random().Next(100000, 999999).ToString();

                var cacheDto = new ResetPasswordCacheDto
                {
                    Email = user.Email!,
                    OtpCode = otpCode
                };

                await _cacheService.SetAsync(resetToken, cacheDto, TimeSpan.FromMinutes(10), cancellationToken);

                var subject = "Password Reset Request";
                var body = $@"
                    <h3>Hello {user.FirstName},</h3>
                    <p>Your OTP to reset your password is: <strong>{otpCode}</strong></p>
                    <p>This code is valid for 10 minutes. If you didn't request this, please ignore this email.</p>";

                await _emailService.SendEmailAsync(user.Email!, subject, body, cancellationToken);
            }

            return ApiResponse<string>.Success(resetToken, "If your email is registered, you will receive an OTP shortly.");
        }
    }
}