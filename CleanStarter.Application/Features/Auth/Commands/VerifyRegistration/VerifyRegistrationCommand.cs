using CleanStarter.Application.Common;
using CleanStarter.Application.Dtos.AuthModel;
using MediatR;

namespace CleanStarter.Application.Features.Auth.Commands.VerifyRegistration
{
    public class VerifyRegistrationCommand : IRequest<ApiResponse<AuthModel>>
    {
        public VerifyRegistrationRequest Model { get; set; }

        public VerifyRegistrationCommand(VerifyRegistrationRequest model)
        {
            Model = model;
        }
    }
}