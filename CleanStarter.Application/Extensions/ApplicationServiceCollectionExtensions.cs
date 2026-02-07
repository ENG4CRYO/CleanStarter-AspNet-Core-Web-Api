using AutoMapper;
using FluentValidation;
using CleanStarter.Application.Helpers;
using CleanStarter.Application.Interfaces;
using CleanStarter.Application.Profiles;
using CleanStarter.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanStarter.Application.Extensions
{
    public static class ApplicationServiceCollectionExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<JWT>();
            services.AddScoped<TokenHelper>();
            services.AddAutoMapper(cfg => cfg.AddProfile<AuthProfile>());

            services.AddValidatorsFromAssembly(typeof(ApplicationServiceCollectionExtensions).Assembly);

        }
    }
}
