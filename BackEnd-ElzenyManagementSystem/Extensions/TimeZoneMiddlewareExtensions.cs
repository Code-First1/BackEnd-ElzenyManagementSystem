public static class TimeZoneMiddlewareExtensions
{
    public static IApplicationBuilder UseEgyptTimeZone(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<TimeZoneMiddleware>();
    }
}
