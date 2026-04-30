using CleanStarter.Application.Common;
using CleanStarter.Application.Dtos.AuthModel;
using MediatR;

namespace CleanStarter.Application.Features.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommand : IRequest<ApiResponse<AuthModel>>
    {
        public RequestRefreshToken Model { get; set; }

        public RefreshTokenCommand(RequestRefreshToken model)
        {
            Model = model;
        }
    }
}