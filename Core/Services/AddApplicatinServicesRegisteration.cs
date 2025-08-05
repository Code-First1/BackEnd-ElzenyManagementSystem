using Microsoft.Extensions.DependencyInjection;
using Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public static class AddApplicatinServicesRegisteration
    {
        public static IServiceCollection AddApplicatinServices(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg => cfg.AddMaps(typeof(AssemblyRef).Assembly));
            services.AddTransient<PictureUrlResolver>();
            services.AddScoped<IServiceManager, ServiceManager>();

            return services;
        }
    }
}
