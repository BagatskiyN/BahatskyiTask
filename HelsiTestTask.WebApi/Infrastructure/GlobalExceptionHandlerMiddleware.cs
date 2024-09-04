using HelsiTestTask.Domain.Constants;
using HelsiTestTask.Domain.Responses;
using Newtonsoft.Json;
using System.Net;

namespace HelsiTestTask.WebApi.Infrastructure
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var error = GetExceptionDetails(ex);

                var result = JsonConvert.SerializeObject(ex);

                var routeData = context.GetRouteData();

                _logger.Log(LogLevel.Error, ex, $"{routeData?.Values["controller"]} { routeData?.Values["action"]} - { error.Message }");

                context.Response.Clear();

                context.Response.ContentType = ApplicationContentTypes.ApplicationJson;

                context.Response.StatusCode = error.StatusCode;

                context.Response.ContentLength = result.Length;

                await context.Response.WriteAsync(result);
            }
        }

        private ErrorResponse GetExceptionDetails(Exception exception)
        {
            var error = new ErrorResponse
            {
                Message = exception.Message,
                StatusCode = (int)HttpStatusCode.InternalServerError,
            };

            error.StatusCode = exception switch
            {
                UnauthorizedAccessException => StatusCodes.Status403Forbidden,
                KeyNotFoundException => StatusCodes.Status404NotFound,
                ArgumentException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError,
            };

            return error;
        }
    }

}
