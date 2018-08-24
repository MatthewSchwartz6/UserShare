using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using static app.Startup;

namespace app.Middleware
{
    public class CustomMiddleware
    {
        private readonly RequestDelegate _next;
        public CustomMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext httpContext)
        {
            Console.WriteLine($" ->>>>>>>>> Request asked for { httpContext.Request.Path}");
            await _next.Invoke(httpContext);
        }
    }
}
