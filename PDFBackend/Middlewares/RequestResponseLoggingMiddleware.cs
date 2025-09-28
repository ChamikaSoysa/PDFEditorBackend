namespace PDFBackend.Middlewares
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

        public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Log request
            _logger.LogInformation("Request: {Method} {Url}", context.Request.Method, context.Request.Path);

            // Enable buffering to read response body
            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await _next(context);

            // Log response
            if (context.Response.StatusCode >= 400)
            {
                _logger.LogWarning("Response: {StatusCode} for {Url}", context.Response.StatusCode, context.Request.Path);
            }

            // Copy response back
            responseBody.Seek(0, SeekOrigin.Begin);
            await responseBody.CopyToAsync(originalBodyStream);
            context.Response.Body = originalBodyStream;
        }
    }
}
