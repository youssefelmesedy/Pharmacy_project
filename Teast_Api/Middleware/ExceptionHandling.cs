using System.ComponentModel.DataAnnotations;

namespace Teast_Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            var response = context.Response;

            // إعداد بيانات الخطأ الافتراضية
            var statusCode = HttpStatusCode.InternalServerError; // 500 كافتراضي
            var message = "An unexpected error occurred. Please try again later.";
            var details = _env.IsDevelopment() ? ex.StackTrace : null; // أظهر التفاصيل فقط في بيئة التطوير

            switch (ex)
            {
                case KeyNotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    message = ex.Message;
                    break;

                case ArgumentException:
                    statusCode = HttpStatusCode.BadRequest; // 400
                    message = ex.Message;
                    break;

                case ValidationException validationEx:
                    statusCode = HttpStatusCode.UnprocessableEntity; // 422
                    message = validationEx.Message;
                    break;

                default:
                    _logger.LogError(ex, "❌ Unexpected error occurred!");
                    break;
            }

            response.StatusCode = (int)statusCode;

            var errorResponse = new
            {
                status = response.StatusCode,
                error = statusCode.ToString(),
                message,
                details
            };

            var json = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            await response.WriteAsync(json);
        }
    }

}
