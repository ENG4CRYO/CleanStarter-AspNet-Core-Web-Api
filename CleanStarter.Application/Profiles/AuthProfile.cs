using AutoMapper;
using CleanStarter.Core.Entities;
using CleanStarter.Application.Dtos.AuthModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanStarter.Application.Profiles
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            
            CreateMap<RegisterModel, ApplicationUser>();
            CreateMap<AuthModel, ApplicationUser>();
            CreateMap<ApplicationUser, AuthModel>();
        }
    }
}
