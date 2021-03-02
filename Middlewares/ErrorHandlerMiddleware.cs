using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace DerekHoneycutt.Middlewares
{
    /// <summary>
    /// Middleware for intercepting and handling exceptions in a potentially meaningful way
    /// </summary>
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate Next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            Next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            //Immediate call Next but catch all exceptions raised
            try
            {
                await Next(context);
            }
            catch (Exception error)
            {
                //Write the exception message to the HTTP return
                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = error switch
                {
                    KeyNotFoundException => (int)HttpStatusCode.NotFound,
                    _ => (int)HttpStatusCode.InternalServerError,
                };

                var result = JsonSerializer.Serialize(new { message = error?.Message });
                await response.WriteAsync(result);
            }
        }
    }
}
