namespace web_api.Middlewares
{
    public class MiddlewareLogging
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<MiddlewareLogging> _logger;

        public MiddlewareLogging(RequestDelegate next, ILogger<MiddlewareLogging> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {

        }
    }
}
