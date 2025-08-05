using Domain.Exceptions;
using Shared.ErrorModels;

namespace BackEnd_ElzenyManagementSystem.Middlewares
{
    public class GlobalErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalErrorHandlingMiddleware> _logger;
        public GlobalErrorHandlingMiddleware(RequestDelegate next, ILogger<GlobalErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
                if(context.Response.StatusCode == StatusCodes.Status404NotFound)
                {
                    await HandlingNotFoundEndPointsAsync(context);
                }
            }
            catch (Exception ex)
            {
                // Log Exception  (Display Console or Store In DB or File)
                _logger.LogError(ex, ex.Message);

                await HandlingErrorAsync(context, ex);

            }
        }

        private static async Task HandlingErrorAsync(HttpContext context, Exception ex)
        {
            // 1.Set Status Code For Response
            // 2. Set Content Typex
            // 3. Response Object (Body)
            // 4. Return Repsonse

            context.Response.ContentType = "application/json";

            var response = new ErrorDetails()
            {
                ErrorMessage = ex.Message
            };
            response.StatusCode = ex switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError
            };

            context.Response.StatusCode = response.StatusCode;

            await context.Response.WriteAsJsonAsync(response);
        }

        private static async Task HandlingNotFoundEndPointsAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            var response = new ErrorDetails()
            {
                StatusCode = StatusCodes.Status404NotFound,
                ErrorMessage = $"End Point {context.Request.Path} is Not Found"
            };
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
