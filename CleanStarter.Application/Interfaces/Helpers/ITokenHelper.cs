using CleanStarter.Core.Entities;
using CleanStarter.Core.Entities.AuthEntites;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace CleanStarter.Application.Interfaces.Helpers
{
    public interface ITokenHelper
    {
        Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user);
        RefreshToken GenerateRefreshToken();
        Task ManageUserTokensAsync(string userId, CancellationToken cancellationToken);
    }
}
