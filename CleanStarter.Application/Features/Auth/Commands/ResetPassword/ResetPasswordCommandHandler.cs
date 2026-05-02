using CleanStarter.Application.Common;
using CleanStarter.Application.Dtos.AuthModel;
using CleanStarter.Application.Interfaces.Infrastructure;
using CleanStarter.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CleanStarter.Application.Features.Auth.Commands.ResetPassword
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, ApiResponse<bool>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICacheService _cacheService;

        public ResetPasswordCommandHandler(UserManager<ApplicationUser> userManager, ICacheService cacheService)
        {
            _userManager = userManager;
            _cacheService = cacheService;
        }

        public async Task<ApiResponse<bool>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var cacheData = await _cacheService.GetAsync<ResetPasswordCacheDto>(request.Model.ResetToken, cancellationToken);

            if (cacheData == null)
            {
                return ApiResponse<bool>.Failure("Session expired or invalid token. Please request a new password reset.");
            }

            if (cacheData.OtpCode != request.Model.OtpCode)
            {
                return ApiResponse<bool>.Failure("Invalid OTP code.");
            }
            var user = await _userManager.FindByEmailAsync(cacheData.Email);
            if (user == null)
            {
                return ApiResponse<bool>.Failure("User not found.");
            }

        
            var identityResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetResult = await _userManager.ResetPasswordAsync(user, identityResetToken, request.Model.NewPassword);

            if (!resetResult.Succeeded)
            {
                var errors = string.Join(" | ", resetResult.Errors.Select(e => e.Description));
                return ApiResponse<bool>.Failure($"Failed to reset password: {errors}");
            }

            await _cacheService.RemoveAsync(request.Model.ResetToken, cancellationToken);

            return ApiResponse<bool>.Success(true, "Password has been reset successfully. You can now login.");
        }
    }
}