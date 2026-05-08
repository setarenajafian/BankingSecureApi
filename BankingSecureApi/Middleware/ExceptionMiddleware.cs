using System.Net;

namespace BankingSecureApi.Middleware;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ExceptionMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // اجازه اجرای درخواست ادامه‌دار را بده
            await _next(context);
        }
        catch (Exception ex)
        {
            // لاگ‌گیری امن – بدون نمایش جزئیات حساس
            _logger.LogError(ex, "unhandled exception occurred");

            // ایجاد پاسخ استاندارد
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var response = new { message = "An unexpected error occurred" };

            await context.Response.WriteAsJsonAsync(response);
        }
    }

   
}
