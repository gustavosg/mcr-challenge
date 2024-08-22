using Api.Middleware;

namespace Api.Configuration
{
    public static class HttpResponseLoggingCfg
    {
        public static IApplicationBuilder UseHttpContextLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HttpContextLoggingMiddleware>();
        }
    }
}
