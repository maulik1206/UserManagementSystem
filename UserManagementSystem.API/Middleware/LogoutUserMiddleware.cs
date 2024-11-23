using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using UserManagementSystem.Application.Dtos;

namespace UserManagementSystem.API.Middleware
{
    public class LogoutUserMiddleware
    {

        private readonly RequestDelegate _next;
        private static List<string> _blacklist = new List<string>();
        public LogoutUserMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext _httpContext)
        {
            // Extract the JWT ID from the incoming token
            string jwtId = string.Empty; 

            var authHeader = _httpContext.Request.Headers["Authorization"].ToString();
            if (authHeader.StartsWith("Bearer "))
            {
                jwtId = authHeader.Substring("Bearer ".Length).Trim();
            }
            else
            {
                jwtId = authHeader;
            }

            if (!string.IsNullOrEmpty(jwtId))
            {
                // Check if the JWT ID is in the blacklist
                if (IsBlacklisted(jwtId))
                {
                    _httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    var result = JsonConvert.SerializeObject(new BaseVM(HttpStatusCode.Unauthorized,
                            new ResponseMessage { Message = "Unauthorized - Token revoked", Description = "" }));
                    await _httpContext.Response.WriteAsync(result);
                    return;
                }
            }            

            // Continue to the next middleware in the pipeline
            await _next.Invoke(_httpContext);
        }

        public static void AddToBlacklist(string jwtId)
        {
            _blacklist.Add(jwtId);
        }

        public static bool IsBlacklisted(string jwtId)
        {
            return _blacklist.Contains(jwtId);
        }
    }
}
