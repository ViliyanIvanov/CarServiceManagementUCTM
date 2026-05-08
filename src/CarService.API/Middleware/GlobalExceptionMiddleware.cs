using System.Net;
using System.Text.Json;
using CarService.Application.Exceptions;
using ValidationException = CarService.Application.Exceptions.ValidationException;

namespace CarService.API.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
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

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        object payload;

        switch (exception)
        {
            case NotFoundException notFound:
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                payload = new { status = 404, message = notFound.Message };
                break;

            case BusinessException business:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                payload = new { status = 400, message = business.Message };
                break;

            case ValidationException validation:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                payload = new { status = 400, message = validation.Message, errors = validation.Errors };
                break;

            default:
                _logger.LogError(exception, "Unhandled exception");
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                payload = new { status = 500, message = "An unexpected error occurred." };
                break;
        }

        var json = JsonSerializer.Serialize(payload);
        await context.Response.WriteAsync(json);
    }
}
