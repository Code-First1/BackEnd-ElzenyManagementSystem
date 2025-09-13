using BackEnd_ElzenyManagementSystem.Extensions;
using BackEnd_ElzenyManagementSystem.Middlewares;
using Domain.Contracts;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using Persistence.Data;
using Persistence.Repositories;
using Services;
using Services.Abstractions;
using Shared.ErrorModels;
using System.Threading.Tasks;
using IInvoiceService = BackEnd_ElzenyManagementSystem.Extensions.IInvoiceService;

namespace BackEnd_ElzenyManagementSystem
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add Services To The Container
            builder.Services.RegisterAllServices(builder.Configuration);

           
            builder.Services.AddHangfire(config =>
                config.UseMemoryStorage());
            builder.Services.AddHangfireServer();

            var app = builder.Build();

            // Configre the HTTP request pipeline
            await app.ConfigureAllMiddlewares();

            // Hangfire Dashboard (UI)
            app.UseHangfireDashboard();

     
            RecurringJob.AddOrUpdate<IInvoiceService>(
                "reset-invoice-number", 
                service => service.ResetInvoiceNumberAsync(),
                "0 0 * * *" 
            );

            app.Run();
        }
    }
}
