using CleanStarter.Application.Common;
using CleanStarter.Application.Dtos.AuthModel;
using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using FluentValidation;
using AutoMapper.Configuration;


namespace CleanStarter.Application.Features.Auth.Commands.Login
{

    public class LoginCommand : IRequest<ApiResponse<AuthModel>>
    {
        public LoginRequest Model { get; set; }
        public LoginCommand(LoginRequest model )
        {
            Model = model;
        }
    }

}