using CleanStarter.Application.Common;
using CleanStarter.Application.Dtos.AuthModel;
using CleanStarter.Application.Interfaces.Infrastructure;
using CleanStarter.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace CleanStarter.Application.Features.Auth.Commands.InitiateRegistration
{
    public class InitiateRegistrationCommandHandler : IRequestHandler<InitiateRegistrationCommand, ApiResponse<string>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICacheService _cacheService;
        private readonly IEmailService _emailService;

        public InitiateRegistrationCommandHandler(
            UserManager<ApplicationUser> userManager,
            ICacheService cacheService,
            IEmailService emailService)
        {
            _userManager = userManager;
            _cacheService = cacheService;
            _emailService = emailService;
        }

        public async Task<ApiResponse<string>> Handle(InitiateRegistrationCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Model.Email);
            if (existingUser != null)
            {
                return ApiResponse<string>.Failure("Email is already registered.");
            }

            var existingUsername = await _userManager.FindByNameAsync(request.Model.Username);
            if (existingUsername != null)
            {
                return ApiResponse<string>.Failure("Username is already taken.");
            }

            var registerToken = Guid.NewGuid().ToString();
            var otpCode = RandomNumberGenerator.GetInt32(100000, 1000000).ToString();

            var passwordHash = _userManager.PasswordHasher.HashPassword(null!, request.Model.Password);

            var pendingUser = new PendingRegistrationDto
            {
                FirstName = request.Model.FirstName,
                LastName = request.Model.LastName,
                Email = request.Model.Email,
                Username = request.Model.Username,
                PasswordHash = passwordHash,
                OtpCode = otpCode
            };

            await _cacheService.SetAsync(registerToken, pendingUser, TimeSpan.FromMinutes(10), cancellationToken);

           
            var subject = "Verification Code for CleanStarter";
            var body = $@"
                <h3>Welcome {request.Model.FirstName}!</h3>
                <p>Your OTP for registration is: <strong>{otpCode}</strong></p>
                <p>This code will expire in 10 minutes.</p>";

            await _emailService.SendEmailAsync(request.Model.Email, subject, body, cancellationToken);

         
            return ApiResponse<string>.Success(registerToken, "OTP has been sent to your email. Please verify.");
        }
    }
}