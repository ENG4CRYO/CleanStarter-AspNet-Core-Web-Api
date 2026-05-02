using CleanStarter.Application.Common;
using CleanStarter.Application.Dtos.AuthModel;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanStarter.Application.Features.Auth.Commands.ResetPassword
{
    public class ResetPasswordCommand : IRequest<ApiResponse<bool>>
    {
        public ResetPasswordRequest Model { get; set; }
        public ResetPasswordCommand(ResetPasswordRequest model)
        {
            Model = model; 
        }
    }
}
