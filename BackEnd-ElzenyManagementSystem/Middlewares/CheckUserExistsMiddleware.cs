using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence.Identity;
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
        if (context.User?.Identity?.IsAuthenticated == true)
        {
            
            var username = context.User.Claims.FirstOrDefault(c =>
                c.Type == "name" ||
                c.Type == "user_name")?.Value;

            if (!string.IsNullOrEmpty(username))
            {
                var exists = await db.Users.AnyAsync(u => u.UserName == username);
                if (!exists)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("User not found or deleted");
                    return;
                }
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
