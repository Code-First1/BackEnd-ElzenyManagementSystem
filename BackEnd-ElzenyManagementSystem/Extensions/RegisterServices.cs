using Domain.Models.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using Persistence.Identity;
using Services;
using Shared.ErrorModels;
using Shared.Options;
using System.Text;

namespace BackEnd_ElzenyManagementSystem.Extensions
{
    public static class RegisterServices
    {
        public static IServiceCollection RegisterAllServices(this IServiceCollection services,IConfiguration configuration)
        {
            // Add services to the container.

            services.AddBuiltInServices();
            services.AddSwaggerServices();

            services.AddInfrastructureServices(configuration);
            services.AddIdentityService();

            services.AddApplicatinServices(configuration);

            services.ConfigureJwtServices(configuration);

            services.ConfigureServices();


            return services;
        }

        private static IServiceCollection AddBuiltInServices(this IServiceCollection services)
        {
            services.AddControllers();

            return services;
        }

        private static IServiceCollection ConfigureJwtServices(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtOptions = configuration.GetSection("JwtOptions").Get<JwtOptions>();

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,

                        ValidIssuer = jwtOptions.Issuer,
                        ValidAudience = jwtOptions.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
                    };
                });
            return services;
        }
        private static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            return services;
        }
        private static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(config =>
            {
                config.InvalidModelStateResponseFactory = (actioncontext) =>
                {
                    var errors = actioncontext.ModelState
                                    .Where(m => m.Value.Errors.Any())
                                    .Select(m => new ValidationError()
                                    {
                                        Field = m.Key,
                                        Errors = m.Value.Errors.Select(errors => errors.ErrorMessage)
                                    });

                    var response = new ValidationErrorResponse()
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(response);
                };
            });

            return services;
        }

        private static IServiceCollection AddIdentityService(this IServiceCollection services)
        {
            services
                .AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores <ElzenyIdentityDbContext>();

            return services;
        }

    }
}
