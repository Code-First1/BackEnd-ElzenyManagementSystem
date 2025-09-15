using Domain.Contracts;

namespace BackEnd_ElzenyManagementSystem.Middlewares
{
    public static class ConfigureMiddlewares
    {
        public static async Task<WebApplication> ConfigureAllMiddlewares(this WebApplication app)
        {

            await app.InitializeDatabaseAsync();


            app.UseGlobalErrorHandling();

            //Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("AllowAll");
            app.UseAuthentication();
            // defined middleware to reject any deleted user
            app.UseCheckUserExists();
            app.UseAuthorization();


            app.MapControllers();

            return app;
        }

        private static async Task<WebApplication> InitializeDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
            await dbInitializer.InitializeAsync();
            await dbInitializer.InitializeIdentityAsync();

            return app;
        }

        private static WebApplication UseGlobalErrorHandling(this WebApplication app)
        {
            app.UseMiddleware<GlobalErrorHandlingMiddleware>();

            return app;
        }
    }
}
