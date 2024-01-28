using Claims.API.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace Claims.API;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, $"Exception occurred: {exception.Message}");

        httpContext.Response.StatusCode = exception switch
        {
            BadRequestException => (int)HttpStatusCode.BadRequest,
            NotFoundException => (int)HttpStatusCode.NotFound,
            _ => (int)HttpStatusCode.InternalServerError,
        };

        await httpContext.Response.WriteAsJsonAsync(exception.Message, cancellationToken);

        return true;
    }
}