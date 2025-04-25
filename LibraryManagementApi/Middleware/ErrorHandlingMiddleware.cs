using System.Net;
using System.Text.Json;
using LibraryManagementApi.Models;   // eðer özel bir ErrorDetails modeli kullanýlacaksa
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace LibraryManagementApi.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ErrorHandlingMiddleware(RequestDelegate next,
                                       ILogger<ErrorHandlingMiddleware> logger,
                                       IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception caught by middleware");
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var code = HttpStatusCode.InternalServerError; // 500 if unexpected

            // Dilersen burada farklý exception tiplerine göre kod set edebilirsin:
            // if (ex is NotFoundException) code = HttpStatusCode.NotFound;

            var result = JsonSerializer.Serialize(new
            {
                statusCode = (int)code,
                message = "An unexpected error occurred.",
                // Geliþtirme ortamýndaysak detay da dönebiliriz:
                details = _env.IsDevelopment() ? ex.Message : null
            });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}
