public class TimeZoneMiddleware
{
    private readonly RequestDelegate _next;
    private readonly TimeZoneInfo _timeZone;

    public TimeZoneMiddleware(RequestDelegate next)
    {
        _next = next;
        _timeZone = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");
    }

    public async Task InvokeAsync(HttpContext context)
    {
      
        context.Items["Now"] = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _timeZone);

        await _next(context);
    }
}
