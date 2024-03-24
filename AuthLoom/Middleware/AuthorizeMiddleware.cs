using AuthLoom.Jwt;
using AuthLoom.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Text.Json;

namespace AuthLoom.Middleware
{
    public class AuthorizeMiddleware(IConfiguration configuration) : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {

            var authConfig = configuration.GetSection("Auth");
            var authSettings = authConfig.Get<AuthSettings>();
            var jwtInfo = GetTokenInfo(context, authSettings!.Jwt.Secret);
            var authorize = await Authorize(context, jwtInfo);
            if (authorize)
            {
                await next(context);
            }
            else
            {
                await SendUnauthorizedResponse(context);
            }
        }

        private async Task<bool> Authorize(HttpContext context, JwtInfo jwtInfo)
        {
            return true;
        }

        private JwtInfo GetTokenInfo(HttpContext context, string secret)
        {
            var authHeader = context.Request.Headers["Authorization"].First() ?? throw new Exception("authorization header not found");
            var token = authHeader.Replace("bearer", "").Trim();
            return JwtUtil.GetTokenInfo(token, secret);
        }

        private async Task SendUnauthorizedResponse(HttpContext context)
        {
            string message = "user unauthorized to perform this action";
            var response = JsonSerializer.Serialize(message);
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await context.Response.WriteAsync(response);
        }
    }
}