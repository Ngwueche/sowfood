using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace SowFoodProject.Middlewares
{
    using System.Net;
    using System.Text.Json;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using static SowFoodProject.Application.DTOs.BaseApiResponse;

    public class GlobalErrorHandler
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private readonly ILogger<GlobalErrorHandler> _logger;

        public GlobalErrorHandler(RequestDelegate next, IConfiguration configuration, ILogger<GlobalErrorHandler> logger)
        {
            _next = next;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception caught in global error handler.");

                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var errorResponse = new ApiResponse
                {
                    IsSuccessful = false,
                    ResponseMessage = "Service unavailable. Try again later.",
                    ResponseCode = "99"
                };

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                await httpContext.Response.WriteAsync(JsonSerializer.Serialize(errorResponse, options));
            }
        }
    }


}

