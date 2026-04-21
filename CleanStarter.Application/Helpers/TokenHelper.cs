using CleanStarter.Application.Interfaces;
using CleanStarter.Application.Interfaces.RepositoryInterfaces;
using CleanStarter.Application.Interfaces.RepositoryInterfaces.Read;
using CleanStarter.Application.Interfaces.RepositoryInterfaces.Write;
using CleanStarter.Core.Entities;
using CleanStarter.Core.Entities.AuthEntites;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CleanStarter.Application.Helpers
{
    public class TokenHelper
    {
        private readonly JWT _jwt;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGenericReadRepository<RefreshToken, int> _refreshTokenReadRepo;
        private readonly IGenericWriteRepository<RefreshToken, int> _refreshTokenWriteRepo;
        private readonly IUnitOfWork _unitOfWork;
        public TokenHelper(IOptions<JWT> jwt,
            UserManager<ApplicationUser> userManager,
            IGenericReadRepository <RefreshToken,int> refreshTokenReadRepo,
            IGenericWriteRepository <RefreshToken,int> refreshTokenWriteRepo,
            IUnitOfWork unitOfWork)
        {
            _jwt = jwt.Value;
            _userManager = userManager;
            _refreshTokenReadRepo = refreshTokenReadRepo;
            _refreshTokenWriteRepo = refreshTokenWriteRepo;
            _unitOfWork = unitOfWork;
        }
        public async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {

            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = new List<Claim>();

            foreach (var role in roles)
            {
                roleClaims.Add(new Claim("roles", role));
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim("uid", user.Id),
                new Claim("fullName", user.FullName)
            }.Union(userClaims).Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwt.AccessTokenValidityInMinutes),
                signingCredentials: signingCredentials
                );

            return jwtSecurityToken;
        }

        public RefreshToken GenerateRefreshToken()
        {
            var randomBytes = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);

            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomBytes),
                Expires = DateTime.UtcNow.AddDays(_jwt.RefreshTokenValidityInDays),
                Created = DateTime.UtcNow
            };


        }

        public async Task ManageUserTokensAsync(string userId)
        {
            var expiredTokens = await _refreshTokenReadRepo.ListAsync(t => t.UserId == userId && t.Expires <= DateTime.UtcNow)
                ?? Enumerable.Empty<RefreshToken>(); ;
            if (expiredTokens.Any())
            {
                await _refreshTokenWriteRepo.DeleteRangeAsync(expiredTokens);
                await _unitOfWork.SaveChangesAsync();
            }

            const int MaxActiveSessions = 5;

            var activeTokens = await _refreshTokenReadRepo.ListAsync(t =>
                t.UserId == userId && t.Revoked == null && t.Expires > DateTime.UtcNow);

            if (activeTokens.Count >= MaxActiveSessions)
            {

                var tokensToRevokeCount = activeTokens.Count - MaxActiveSessions + 1;

                var tokensToRevoke = activeTokens
                    .OrderBy(t => t.Created) 
                    .Take(tokensToRevokeCount)
                    .ToList();

                foreach (var token in tokensToRevoke)
                {
                    token.Revoked = DateTime.UtcNow;
                    token.ReasonRevoked = "Exceeded max active sessions";
                    await _refreshTokenWriteRepo.UpdateAsync(token);
                    await _unitOfWork.SaveChangesAsync();
                }
            }
        }


    }
}
