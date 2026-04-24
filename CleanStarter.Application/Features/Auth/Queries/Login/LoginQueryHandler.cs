using AutoMapper;
using CleanStarter.Application.Common;
using CleanStarter.Application.Dtos.AuthModel;
using CleanStarter.Application.Helpers;
using CleanStarter.Application.Interfaces.Common;
using CleanStarter.Application.Interfaces.Helpers;
using CleanStarter.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace CleanStarter.Application.Features.Auth.Queries.Login
{
#if IsCQRS
    public class LoginQueryHandler : IRequestHandler<LoginQuery, ApiResponse<AuthModel>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenHelper _tokenHelper;
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public LoginQueryHandler(
            UserManager<ApplicationUser> userManager,
            ITokenHelper tokenHelper,
            IMapper mapper,
            IApplicationDbContext context)
        {
            _userManager = userManager;
            _tokenHelper = tokenHelper;
            _mapper = mapper;
            _context = context;
        }

        public async Task<ApiResponse<AuthModel>> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var authModel = new AuthModel();
            var user = await _userManager.FindByEmailAsync(request.Model.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Model.Password))
            {
                authModel.IsAuthenticated = false;
                var failedResponse = ApiResponse<AuthModel>.Failure("Email or Password is incorrect");
                failedResponse.Data = authModel;
                return failedResponse;
            }

            var jwtSecurityToken = await _tokenHelper.CreateJwtToken(user);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            await _tokenHelper.ManageUserTokensAsync(user.Id);

            var refreshToken = _tokenHelper.GenerateRefreshToken();
            refreshToken.UserId = user.Id;

            await _context.RefreshTokens.AddAsync(refreshToken, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var roles = await _userManager.GetRolesAsync(user);
            authModel = _mapper.Map<AuthModel>(user);


            authModel.IsAuthenticated = true;
            authModel.Token = tokenString;
            authModel.RefreshToken = refreshToken.Token;
            authModel.ExpiresOn = jwtSecurityToken.ValidTo;
            authModel.RefreshTokenExpiration = refreshToken.Expires;
            authModel.Roles = roles;

            return ApiResponse<AuthModel>.Success(authModel);
        }
    }
#endif
}