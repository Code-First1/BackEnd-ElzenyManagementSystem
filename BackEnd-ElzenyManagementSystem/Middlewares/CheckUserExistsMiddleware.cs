using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Persistence.Identity;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

public class CheckUserExistsMiddleware
{
    private readonly RequestDelegate _next;

    public CheckUserExistsMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ElzenyIdentityDbContext db)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (!string.IsNullOrEmpty(token))
        {
            var jwtHandler = new JwtSecurityTokenHandler();

            try
            {
                var jwtToken = jwtHandler.ReadJwtToken(token);
                var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "nameid")?.Value;

                if (userId != null)
                {
                    var exists = await db.Users.AnyAsync(u => u.Id == userId);
                    if (!exists)
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsync("User not found or deleted");
                        return;
                    }
                }
            }
            catch
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Invalid Token");
                return;
            }
        }

        await _next(context);
    }
}

// Extension
public static class CheckUserExistsMiddlewareExtensions
{
    public static IApplicationBuilder UseCheckUserExists(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CheckUserExistsMiddleware>();
    }
}
