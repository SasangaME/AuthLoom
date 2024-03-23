using AuthLoom.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace AuthLoom.Middleware
{
    public class AuthorizeMiddleware(IConfiguration configuration) : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {

            var authConfig = configuration.GetSection("Auth");
            var authSettings = authConfig.Get<AuthSettings>();

            await next(context);
        }
    }
}