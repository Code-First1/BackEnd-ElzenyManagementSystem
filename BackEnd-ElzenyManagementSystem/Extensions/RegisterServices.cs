using Microsoft.AspNetCore.Mvc;
using Persistence;
using Services;
using Shared.ErrorModels;

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
            services.AddApplicatinServices();

            services.ConfigureServices();


            return services;
        }

        private static IServiceCollection AddBuiltInServices(this IServiceCollection services)
        {
            services.AddControllers();

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
    }
}
