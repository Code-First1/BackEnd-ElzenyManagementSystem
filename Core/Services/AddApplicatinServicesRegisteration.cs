using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.Abstractions;
using Shared.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public static class AddApplicatinServicesRegisteration
    {
        public static IServiceCollection AddApplicatinServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(cfg => cfg.AddMaps(typeof(AssemblyRef).Assembly));
            services.AddTransient<PictureUrlResolver>();
            services.AddScoped<IServiceManager, ServiceManager>();

            services.Configure<JwtOptions>(configuration.GetSection(key: "JwtOptions"));


            return services;
        }
    }
}
