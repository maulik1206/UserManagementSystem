using Newtonsoft.Json;
using System.Net;
using UserManagementSystem.Application.Dtos;

namespace UserManagementSystem.API.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var result = JsonConvert.SerializeObject(new BaseVM(HttpStatusCode.InternalServerError,
                new ResponseMessage { Message = exception.Message, Description = "" }));
            return context.Response.WriteAsync(result);
        }
    }
}
