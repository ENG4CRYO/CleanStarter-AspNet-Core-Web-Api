using CleanStarter.Application.Common;
using CleanStarter.Application.Dtos.AuthModel;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanStarter.Application.Features.Auth.Commands.InitiateRegistration
{
    public class InitiateRegistrationCommand : IRequest<ApiResponse<string>>
    {
        public InitiateRegistrationRequest Model { get; set; } 
        public InitiateRegistrationCommand(InitiateRegistrationRequest model)
        {
            Model = model;
        }
    }
}
