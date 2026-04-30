using CleanStarter.Application.Common;
using CleanStarter.Application.Dtos.AuthModel;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanStarter.Application.Features.Auth.Commands.RevokeToken
{
    public class RevokeTokenCommand : IRequest<ApiResponse<bool>>
    {
        public RevokeTokenRequest Model { get; set; }
        public RevokeTokenCommand(RevokeTokenRequest model)
        {
            Model = model;
        }
    }
}
