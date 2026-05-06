using CleanStarter.Application.Common;
using CleanStarter.Application.Dtos.AuthModel;
using MediatR;

namespace CleanStarter.Application.Features.Auth.Commands.VerifyRegistration
{
    public class VerifyRegistrationCommand : IRequest<ApiResponse<AuthModel>>
    {
        public string RegisterToken { get; set; } = string.Empty;
        public string OtpCode { get; set; } = string.Empty;
    }
}