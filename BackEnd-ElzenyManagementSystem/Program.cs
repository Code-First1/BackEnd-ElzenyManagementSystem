
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

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<ElzenyDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            }
            );
            builder.Services.AddScoped<IDbInitializer,DbInitializer>(); //Allow DI For DbInitalizer
            builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
            builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(AssemblyRef).Assembly));
            builder.Services.AddTransient<PictureUrlResolver>();
            builder.Services.AddScoped<IServiceManager, ServiceManager>();

            builder.Services.Configure<ApiBehaviorOptions>(config =>
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

            var app = builder.Build();

            using var scope =  app.Services.CreateScope();
            var dbInitializer =  scope.ServiceProvider.GetRequiredService<IDbInitializer>();
            await dbInitializer.InitializeAsync();

            app.UseMiddleware<GlobalErrorHandlingMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
