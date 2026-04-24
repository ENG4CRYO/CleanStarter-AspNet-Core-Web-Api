using Asp.Versioning;
using CleanStarter.api.Factories;
using CleanStarter.Application.Common;
using CleanStarter.Application.Dtos.AuthModel;
using CleanStarter.Application.Features.Auth.Commands.Register;
using CleanStarter.Application.Features.Auth.Queries.Login;
using CleanStarter.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanStarter.API.Controllers.V1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[Controller]")]
    [ApiVersion("1.0")]
    public class AuthController : ControllerBase 
    {
        private readonly IAuthService _authService;
        private readonly IMediator _mediator;

        public AuthController(IAuthService authService, IMediator mediator)
        {
            _authService = authService;
            _mediator = mediator;   
        }

        [HttpPost("register")]
        [EndpointDescription("It creates a new user account and returns the token and user details. It requires a unique email address.")]
        [ProducesResponseType(typeof(ApiResponse<AuthModel>),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<AuthModel>),StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
#if IsCQRS
            var response = await _mediator.Send(new RegisterCommand(model));
#elif IsRepository
            var response = await _authService.RegisterAsync(model);
#endif
            if (!response.Succeeded)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPost("login")]
        [EndpointDescription("Verifying user cridential,Issue Token and refresh token for user")]
        [ProducesResponseType(typeof(ApiResponse<AuthModel>),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<AuthModel>),StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] TokenRequestModel model)
        {

#if IsCQRS
            var response = await _mediator.Send(new LoginQuery(model));
#elif IsRepository
            var response = await _authService.GetTokenAsync(model);
#endif
            if (!response.Succeeded)
                return BadRequest(response);

            return Ok(response);
        }


        [HttpPost("refresh-token")]
        [EndpointDescription("Verify refresh token, rovoke the refresh token, issue new refresh token")]
        [ProducesResponseType(typeof(ApiResponse<AuthModel>),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<AuthModel>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RefreshToken([FromBody] RequestRefreshToken refreshToken)
        {
            var result = await _authService.RefreshTokenAsync(refreshToken.Token);

            if (!result.Succeeded)
                return BadRequest(result);

            return Ok(result);
        }


        [HttpPost("logout")]
        [EndpointDescription("revoke the refresh token")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenRequest model)
        {
          

            if (string.IsNullOrEmpty(model.Token))
                return BadRequest(ApiResponse<string>.Failure("Token is required!"));

            var result = await _authService.RevokeTokenAsync(model.Token);

            if (!result.Succeeded)
                return BadRequest(result);

            return Ok(ApiResponse<string>.Success("Token revoked successfully"));
        }

       
    }
}