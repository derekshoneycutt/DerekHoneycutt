using Microsoft.Azure.Functions.Worker.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DerekHoneycutt.Api
{
    /// <summary>
    /// Handler methods for JSON Responses
    /// </summary>
    internal static class ResponseJsonHandler
    {
        /// <summary>
        /// Set an exception to an HTTP output response
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="response"></param>
        /// <param name="jsonOptions"></param>
        public static void SetExceptionToHttpResponse(
            Exception ex, HttpResponseData response, JsonSerializerOptions jsonOptions)
        {
            string error = "InternalError";
            if (ex is IndexOutOfRangeException iorex)
            {
                error = "BadId";
                response.StatusCode = System.Net.HttpStatusCode.BadRequest;
            }
            else if (ex is KeyNotFoundException nfex)
            {
                error = "NotFound";
                response.StatusCode = System.Net.HttpStatusCode.NotFound;
            }
            else
            {
                response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            }

            var stringBody = JsonSerializer.Serialize(new
            {
                Error = error,
                ex.Message
            }, jsonOptions);
            response.WriteString(stringBody);
        }
    }
}
