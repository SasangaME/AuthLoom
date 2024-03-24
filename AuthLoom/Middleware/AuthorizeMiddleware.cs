using AuthLoom.Jwt;
using AuthLoom.Models;
using Microsoft.AspNetCore.Components;
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
            try
            {
                var authConfig = configuration.GetSection("Auth");
                var authSettings = authConfig.Get<AuthSettings>();
                var jwtInfo = GetTokenInfo(context, authSettings!.Jwt.Secret);
                var authorize = Authorize(context, jwtInfo, authSettings);
                if (authorize)
                {
                    await next(context);
                }
                else
                {
                    await SendUnauthorizedResponse(context);
                }
            }
            catch (Exception ex)
            {
                await SendUnauthorizedResponse(context, ex.Message);
            }
        }

        private bool Authorize(HttpContext context, JwtInfo jwtInfo, AuthSettings authSettings)
        {
            var path = GetPath(context, authSettings.BaseUrl);
            var roles = GetRolesFromConfig(path, authSettings);
            return roles.Exists(role => role == jwtInfo.Role);
        }

        private JwtInfo GetTokenInfo(HttpContext context, string secret)
        {
            var authHeader = context.Request.Headers["Authorization"].First()
                ?? throw new Exception("authorization header not found");
            var token = authHeader.Replace("Bearer", "").Trim();
            return JwtUtil.GetTokenInfo(token, secret);
        }

        private async Task SendUnauthorizedResponse(HttpContext context, string errorMessage = "")
        {
            string message = $"user unauthorized to perform this action. {errorMessage}";
            var response = JsonSerializer.Serialize(message);
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await context.Response.WriteAsync(response);
        }

        private string GetPath(HttpContext context, string baseUrl)
        {
            var path = context.Request.Path.ToString();
            return path.Replace("/api/", "");
        }

        private List<string> GetRolesFromConfig(string path, AuthSettings authSettings)
        {
            var rolesStr = authSettings.Endpoints.FirstOrDefault(q => q.Path == path)!.Roles;
            return rolesStr.Split(',').ToList();
        }
    }
}