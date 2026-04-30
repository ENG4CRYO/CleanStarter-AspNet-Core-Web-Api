using CleanStarter.Core.Entities;
using CleanStarter.Core.Entities.AuthEntites;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CleanStarter.Application.Interfaces.Helpers
{
    public interface ITokenHelper
    {
        JwtSecurityToken CreateJwtToken(ApplicationUser user, IList<string> roles, IList<Claim> userClaims);
        RefreshToken GenerateRefreshToken();
    }
}
