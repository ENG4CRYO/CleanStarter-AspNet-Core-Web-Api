using AutoMapper;
using CleanStarter.Application.Common;
using CleanStarter.Application.Dtos.AuthModel;
using CleanStarter.Application.Interfaces.Helpers;
using CleanStarter.Application.Interfaces.Infrastructure;
using CleanStarter.Core.Constants;
using CleanStarter.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CleanStarter.Application.Features.Auth.Commands.VerifyRegistration
{
    public class VerifyRegistrationCommandHandler : IRequestHandler<VerifyRegistrationCommand, ApiResponse<AuthModel>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICacheService _cacheService;
        private readonly ITokenHelper _tokenHelper;
        private readonly IMapper _mapper;

        public VerifyRegistrationCommandHandler(
            UserManager<ApplicationUser> userManager,
            ICacheService cacheService,
            ITokenHelper tokenHelper,
            IMapper mapper)
        {
            _userManager = userManager;
            _cacheService = cacheService;
            _tokenHelper = tokenHelper;
            _mapper = mapper;
        }

        public async Task<ApiResponse<AuthModel>> Handle(VerifyRegistrationCommand request, CancellationToken cancellationToken)
        {
            var pendingUser = await _cacheService.GetAsync<PendingRegistrationDto>(request.Model.RegisterToken, cancellationToken);

          
            if (pendingUser == null)
            {
                return ApiResponse<AuthModel>.Failure("Session expired or invalid token. Please register again.");
            }

            if (pendingUser.OtpCode != request.Model.OtpCode)
            {
                return ApiResponse<AuthModel>.Failure("Invalid OTP code.");
            }

            var newUser = new ApplicationUser
            {
                FirstName = pendingUser.FirstName,
                LastName = pendingUser.LastName,
                Email = pendingUser.Email,
                UserName = pendingUser.Username,
                EmailConfirmed = true, 
                PasswordHash = pendingUser.PasswordHash 
            };

           
            var result = await _userManager.CreateAsync(newUser);
            if (!result.Succeeded)
            {
                var errorMessages = string.Join(" | ", result.Errors.Select(e => e.Description));
                return ApiResponse<AuthModel>.Failure($"Error occurred while creating account: {errorMessages}");
            }
            await _userManager.AddToRoleAsync(newUser, AspRoles.User);


            var newRefreshToken = _tokenHelper.GenerateRefreshToken();
            newUser.RefreshTokens.Add(newRefreshToken);
            await _userManager.UpdateAsync(newUser);


            var roles = await _userManager.GetRolesAsync(newUser);
            var claims = await _userManager.GetClaimsAsync(newUser);
            var jwtToken = _tokenHelper.CreateJwtToken(newUser, roles, claims);

            
            await _cacheService.RemoveAsync(request.Model.RegisterToken, cancellationToken);


            var authModel = _mapper.Map<AuthModel>(newUser);
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            authModel.RefreshToken = newRefreshToken.Token;
            authModel.ExpiresOn = jwtToken.ValidTo;
            authModel.RefreshTokenExpiration = newRefreshToken.Expires;
            authModel.IsAuthenticated = true;
            authModel.Roles = roles.ToList();

            return ApiResponse<AuthModel>.Success(authModel, "Account verified and created successfully.");
        }
    }
}