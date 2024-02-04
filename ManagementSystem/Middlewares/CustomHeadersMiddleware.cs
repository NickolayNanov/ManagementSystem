namespace ManagementSystem.Middlewares
{
    public class CustomHeadersMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomHeadersMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Response.OnStarting(() =>
            {
                context.Response.Headers["X-Custom-Header"] = "This is my custom header value";
                return Task.CompletedTask;
            });

            await _next(context);
        }
    }
}
