using AutoMapper;
using CleanStarter.Core.Entities;
using CleanStarter.Application.Dtos.AuthModel;
using System;
using System.Collections.Generic;
using System.Text;
using CleanStarter.Application.Features.Auth.Commands.Register;
using CleanStarter.Application.Features.Auth.Commands.InitiateRegistration;

namespace CleanStarter.Application.Profiles
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            
            CreateMap<RegisterCommand, ApplicationUser>();
            CreateMap<AuthModel, ApplicationUser>();
            CreateMap<ApplicationUser, AuthModel>();
            CreateMap<InitiateRegistrationCommand, ApplicationUser>();
        }
    }
}
