using AutoMapper.Configuration;
using CleanStarter.Application.Common;
using CleanStarter.Application.Dtos.AuthModel;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanStarter.Application.Features.Auth.Commands.Register
{
#if IsCQRS
    public class RegisterCommand : IRequest<ApiResponse<AuthModel>>
    {
        public RegisterModel Model { get; set; }

        public RegisterCommand(RegisterModel model)
        {
            Model = model;
        }
    }
#endif
}
