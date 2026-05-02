using CleanStarter.Application.Common;
using CleanStarter.Application.Dtos.AuthModel;
using MediatR;

namespace CleanStarter.Application.Features.Auth.Commands.ForgotPassword
{
    public class ForgotPasswordCommand : IRequest<ApiResponse<string>>
    {
        public ForgotPasswordRequest Model { get; set; }
        public ForgotPasswordCommand(ForgotPasswordRequest model)
        {
            Model = model;
        }
    }
}
