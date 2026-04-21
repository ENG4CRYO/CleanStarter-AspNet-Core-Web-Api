using CleanStarter.Application.Helpers;
using CleanStarter.Application.Interfaces;
using CleanStarter.Application.Interfaces.RepositoryInterfaces.Read;
using CleanStarter.Application.Interfaces.RepositoryInterfaces.Write;
using CleanStarter.Core.Entites;
using CleanStarter.Infrastructure.Data;
using CleanStarter.Infrastructure.Repositories;
using CleanStarter.Infrastructure.Repositories.Read;
using CleanStarter.Infrastructure.Repositories.Write;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanStarter.Infrastructure.Extensions
{
    public static class InfrastructureServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration configuration)
        {
            
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string 'LocalDb' not found.");
            }

            services.AddIdentity<ApplicationUser, IdentityRole>()
                 .AddEntityFrameworkStores<AppDbContext>();

            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(connectionString,b =>
                b.MigrationsAssembly(typeof(InfrastructureServiceCollectionExtensions).Assembly.FullName)
                ));

            services.AddScoped(typeof(IGenericReadRepository<,>), typeof(GenericReadRepository<,>));
            services.AddScoped(typeof(IGenericWriteRepository<,>), typeof(GenericWriteRepository<,>));


            services.Configure<JWT>(configuration.GetSection("JWT"));
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var jwtKey = configuration["JWT:Key"];
                var jwtIssuer = configuration["JWT:Issuer"];
                var jwtAudience = configuration["JWT:Audience"];

                if (string.IsNullOrEmpty(jwtKey))
                {
                    throw new InvalidOperationException("JWT Key is missing from configuration.");
                }

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = jwtIssuer,
                    ValidAudience = jwtAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                };
            });;

            return services;
        }
    }
}
