using AutoMapper;
using CleanStarter.Application.Common.Behaviors;
using CleanStarter.Application.Helpers;
using CleanStarter.Application.Interfaces;
using CleanStarter.Application.Interfaces.Common;
using CleanStarter.Application.Interfaces.Helpers;
using CleanStarter.Application.Profiles;
using CleanStarter.Application.Services;
using CleanStarter.Core.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
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
            services.AddScoped<JWT>();
            services.AddScoped<ITokenHelper, TokenHelper>();
            services.AddAutoMapper(cfg => cfg.AddProfile<AuthProfile>());

            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());


                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });

            services.AddLocalization();




            services.AddValidatorsFromAssembly(typeof(ApplicationServiceCollectionExtensions).Assembly);

        }
    }
}
