using AutoMapper;
using CleanStarter.Application.Common;
using CleanStarter.Application.Dtos.AuthModel;
using CleanStarter.Application.Interfaces.Helpers;
using CleanStarter.Core.Constants;
using CleanStarter.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CleanStarter.Application.Features.Auth.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ApiResponse<AuthModel>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ITokenHelper _tokenHelper;

        public RegisterCommandHandler(
            UserManager<ApplicationUser> userManager,
            IMapper mapper,
            ITokenHelper tokenHelper)
        {
            _userManager = userManager;
            _mapper = mapper;
            _tokenHelper = tokenHelper;
        }

        public async Task<ApiResponse<AuthModel>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var authModel = new AuthModel { IsAuthenticated = false };

            var existingUser = await _userManager.FindByEmailAsync(request.Model.Email);
            if (existingUser != null)
            {
                var failedResponse = ApiResponse<AuthModel>.Failure("Email is already registered");
                failedResponse.Data = authModel;
                return failedResponse;
            }

            var newUser = _mapper.Map<ApplicationUser>(request.Model);

            var result = await _userManager.CreateAsync(newUser, request.Model.Password);
            if (!result.Succeeded)
            {
                var errorMessages = string.Join(" | ", result.Errors.Select(e => e.Description));
                var failedResponse = ApiResponse<AuthModel>.Failure($"Error occurred while creating account: {errorMessages}");
                failedResponse.Data = authModel;
                return failedResponse;
            }

 
            await _userManager.AddToRoleAsync(newUser, AspRoles.User);

            var newRefreshToken = _tokenHelper.GenerateRefreshToken();
            newUser.RefreshTokens.Add(newRefreshToken);

            await _userManager.UpdateAsync(newUser);

            var roles = await _userManager.GetRolesAsync(newUser);
            var claims = await _userManager.GetClaimsAsync(newUser);
            var token = _tokenHelper.CreateJwtToken(newUser, roles, claims);


            authModel = _mapper.Map<AuthModel>(newUser);
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(token);
            authModel.RefreshToken = newRefreshToken.Token;
            authModel.ExpiresOn = token.ValidTo;
            authModel.RefreshTokenExpiration = newRefreshToken.Expires;
            authModel.IsAuthenticated = true;
            authModel.Roles = roles.ToList(); 

            return ApiResponse<AuthModel>.Success(authModel, "User registered successfully");
        }
    }
}