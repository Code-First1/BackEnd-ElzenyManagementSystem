
using BackEnd_ElzenyManagementSystem.Extensions;
using BackEnd_ElzenyManagementSystem.Middlewares;
using Domain.Contracts;
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

namespace BackEnd_ElzenyManagementSystem
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add Services To The Container
            builder.Services.RegisterAllServices(builder.Configuration);

            var app = builder.Build();

            // Configre the HTTP request pipeline
            app.ConfigureAllMiddlewares();

            app.Run();
        }
    }
}
