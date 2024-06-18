using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net;
using WebApi.CustomValidationExceptions;
using WebApi.DTOs.Response;
using WebApi.Helpers;

namespace WebApi.Middleware
{
    public class ErrorHandler
    {
        private readonly RequestDelegate _next;

        public ErrorHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                if (error is CustomValidationException validationException)
                {
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    var errorResponse = new { message = validationException.Message };
                    await response.WriteAsync(JsonConvert.SerializeObject(errorResponse));
                }
                else
                {
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    var errorResponse = new { message = "Something went wrong, your request could not be processed." };
                    await response.WriteAsync(JsonConvert.SerializeObject(errorResponse));
                }

                Console.WriteLine($"Error handler caught exception => {error.StackTrace}");
            }
        }
    }
}
