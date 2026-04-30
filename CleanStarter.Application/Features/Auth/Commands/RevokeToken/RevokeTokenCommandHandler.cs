using CleanStarter.Application.Common;
using CleanStarter.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CleanStarter.Application.Features.Auth.Commands.RevokeToken
{
    public class RevokeTokenCommandHandler : IRequestHandler<RevokeTokenCommand, ApiResponse<bool>>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public RevokeTokenCommandHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ApiResponse<bool>> Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
        {
            var tokenToRevoke = request.Model.Token;

            var user = await _userManager.Users
                .Include(u => u.RefreshTokens)
                .SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == tokenToRevoke), cancellationToken);

            if (user == null)
            {
                return ApiResponse<bool>.Failure("Invalid Token.");
            }

            var refreshToken = user.RefreshTokens.Single(t => t.Token == tokenToRevoke);

            if (!refreshToken.IsActive)
            {
                return ApiResponse<bool>.Failure("Token is already inactive.");
            }

            refreshToken.Revoked= DateTime.UtcNow;

            await _userManager.UpdateAsync(user);

            return ApiResponse<bool>.Success(true, "Token revoked successfully.");
        }
    }
}