using Asp.Versioning;
using CleanStarter.api.Factories;
using CleanStarter.Application.Common;
using CleanStarter.Application.Dtos.AuthModel;
using CleanStarter.Application.Features.Auth.Commands.Register;
using CleanStarter.Application.Features.Auth.Commands.Login;
using CleanStarter.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using CleanStarter.Application.Features.Auth.Commands.RefreshToken;
using CleanStarter.Application.Features.Auth.Commands.RevokeToken;

namespace CleanStarter.API.Controllers.V1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[Controller]")]
    [ApiVersion("1.0")]
    public class AuthController : ControllerBase 
    {
        private readonly IMediator _mediator;

        public AuthController( IMediator mediator)
        {
            _mediator = mediator;   
        }

        [HttpPost("register")]
        [EndpointDescription("It creates a new user account and returns the token and user details. It requires a unique email address.")]
        [ProducesResponseType(typeof(ApiResponse<AuthModel>),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<AuthModel>),StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var response = await _mediator.Send(new RegisterCommand(model));
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
            var response = await _mediator.Send(new LoginCommand(model));
            if (!response.Succeeded)
                return BadRequest(response);

            return Ok(response);
        }


        [HttpPost("refresh-token")]
        [EndpointDescription("Verify refresh token, rovoke the refresh token, issue new refresh token")]
        [ProducesResponseType(typeof(ApiResponse<AuthModel>),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<AuthModel>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RefreshToken([FromBody] RequestRefreshToken refreshToken, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new RefreshTokenCommand(refreshToken));

            if (!response.Succeeded)
                return BadRequest(response);

            return Ok(response);
        }


        [HttpPost("logout")]
        [EndpointDescription("revoke the refresh token")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenRequest model, CancellationToken cancellationToken)
        {
          

            if (string.IsNullOrEmpty(model.Token))
                return BadRequest(ApiResponse<string>.Failure("Token is required!"));

            var response = await _mediator.Send(new RevokeTokenCommand(model));

            if (!response.Succeeded)
                return BadRequest(response);

            return Ok(ApiResponse<string>.Success("Token revoked successfully"));
        }

       
    }
}