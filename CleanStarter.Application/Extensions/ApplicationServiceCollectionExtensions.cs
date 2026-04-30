using AutoMapper;
using CleanStarter.Application.Common.Behaviors;
using CleanStarter.Application.Helpers;
using CleanStarter.Application.Interfaces;
using CleanStarter.Application.Interfaces.Helpers;
using CleanStarter.Application.Profiles;
using CleanStarter.Application.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace CleanStarter.Application.Extensions
{
    public static class ApplicationServiceCollectionExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<JWT>();
            services.AddScoped<ITokenHelper, TokenHelper>();
            services.AddAutoMapper(cfg => cfg.AddProfile<AuthProfile>());

#if IsCQRS
            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

  
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
#endif

            services.AddValidatorsFromAssembly(typeof(ApplicationServiceCollectionExtensions).Assembly);

        }
    }
}
