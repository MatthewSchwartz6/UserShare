using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;


namespace app.Middleware
{
    public static class MiddlewareExtensions
    {
        
        public static IApplicationBuilder UseCustomeMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomMiddleware>();
        }

    }
}
