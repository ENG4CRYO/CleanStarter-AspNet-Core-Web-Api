using AutoMapper;
using CleanStarter.Application.Common;
using CleanStarter.Application.Dtos.AuthModel;
using CleanStarter.Application.Interfaces.Common;
using CleanStarter.Application.Interfaces.Helpers;
using CleanStarter.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace CleanStarter.Application.Features.Auth.Commands.Register
{
#if IsCQRS
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ApiResponse<AuthModel>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ITokenHelper _tokenHelper;
        private readonly IApplicationDbContext _context;

        public RegisterCommandHandler(
            UserManager<ApplicationUser> userManager,
            IMapper mapper,
            ITokenHelper tokenHelper,
            IApplicationDbContext context)
        {
            _userManager = userManager;
            _mapper = mapper;
            _tokenHelper = tokenHelper;
            _context = context;
        }
        public async Task<ApiResponse<AuthModel>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var authModel = new AuthModel();

            var user = await _userManager.FindByEmailAsync(request.Model.Email);
            if (user != null)
            {
                authModel.IsAuthenticated = false;
                var failedResponse = ApiResponse<AuthModel>.Failure("Email is already registered");
                failedResponse.Data = authModel;
                return failedResponse;
            }

            var newUser = _mapper.Map<ApplicationUser>(request.Model);
            newUser.ConcurrencyStamp = Guid.NewGuid().ToString();

            var result = await _userManager.CreateAsync(newUser, request.Model.Password);
            if (!result.Succeeded)
            {
                authModel.IsAuthenticated = false;

                var errorMessages = string.Join(" | ", result.Errors.Select(e => e.Description));

                var failedResponse = ApiResponse<AuthModel>.Failure($"Error occurred while creating account: {errorMessages}");
                failedResponse.Data = authModel;
                return failedResponse;
            }

            await _userManager.AddToRoleAsync(newUser, AspRoles.User);

            var newRefreshToken = _tokenHelper.GenerateRefreshToken();
            newRefreshToken.UserId = newUser.Id;

            await _context.RefreshTokens.AddAsync(newRefreshToken, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var token = await _tokenHelper.CreateJwtToken(newUser);

            authModel = _mapper.Map<AuthModel>(newUser);
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(token);
            authModel.RefreshToken = newRefreshToken.Token;
            authModel.ExpiresOn = token.ValidTo;
            authModel.RefreshTokenExpiration = newRefreshToken.Expires;
            authModel.IsAuthenticated = true;
            authModel.Roles = new List<string>() { AspRoles.User };

            return ApiResponse<AuthModel>.Success(authModel, "User registered successfully");
        }
    }
#endif
}
