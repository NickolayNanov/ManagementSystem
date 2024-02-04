using Microsoft.Extensions.Configuration;
using System.Net;

namespace ManagementSystem.Middlewares
{
    public class IRequestSourceFilterMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<IRequestSourceFilterMiddleware> _logger;
        private readonly List<IPAddress> _allowedIPs;

        public IRequestSourceFilterMiddleware(RequestDelegate next, ILogger<IRequestSourceFilterMiddleware> logger, IConfiguration configuration)
        {
            _next = next;
            _logger = logger;
            _allowedIPs = configuration.GetSection("AllowedIPs")
                                   .AsEnumerable()
                                   .Where(ip => !string.IsNullOrEmpty(ip.Value))
                                   .Select(ip => IPAddress.Parse(ip.Value))
                                   .ToList();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var requestIP = context.Connection.RemoteIpAddress;

            if (_allowedIPs.Contains(requestIP))
            {
                _logger.LogInformation($"Allowed IP: {requestIP}");
                await _next(context);
            }
            else
            {
                _logger.LogWarning($"Blocked IP: {requestIP}");
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Access denied");
            }
        }
    }
}
