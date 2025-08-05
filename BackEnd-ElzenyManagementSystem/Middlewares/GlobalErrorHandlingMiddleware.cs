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
            }
            catch (Exception ex)
            {
                // Log Exception  (Display Console or Store In DB or File)
                _logger.LogError(ex, ex.Message);

                // 1.Set Status Code For Response
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                // 2. Set Content Type
                context.Response.ContentType = "application/json";

                // 3. Response Object (Body)
                var response = new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = ex.Message
                };
                    
                // 4. Return Repsonse
                await context.Response.WriteAsJsonAsync(response);

            }
        }

    }
}
