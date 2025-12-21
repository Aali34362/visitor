namespace Visitor.Core.WebServices.ExceptionHandlers;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {

        _logger.LogError(
            exception, "Exception occurred: {Message}", exception.Message);

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Server error"
        };

        switch (exception)
        {

            case NotImplementedException:
                problemDetails.Status = (int)HttpStatusCode.NotImplemented;
                problemDetails.Title = exception.GetType().Name;
                problemDetails.Detail = exception.Message;
                break;

            case NotFoundException or KeyNotFoundException:
                problemDetails.Status = (int)HttpStatusCode.NotFound;
                problemDetails.Title = exception.GetType().Name;
                problemDetails.Detail = exception.Message;
                break;

            case UnauthorizedAccessException:
                problemDetails.Status = (int)HttpStatusCode.Unauthorized;
                problemDetails.Title = exception.GetType().Name;
                problemDetails.Detail = exception.Message;
                break;

            case ForbiddenAccessException:
                problemDetails.Status = (int)HttpStatusCode.Forbidden;
                problemDetails.Title = exception.GetType().Name;
                problemDetails.Detail = exception.Message;
                break;

            case TimeoutException timeoutException:
                problemDetails.Status = (int)HttpStatusCode.RequestTimeout;
                problemDetails.Title = timeoutException.GetType().Name;
                problemDetails.Detail = "The request has timed out.";
                break;

            default:
                problemDetails.Status = (int)HttpStatusCode.InternalServerError;
                problemDetails.Title = "Internal Server Error";
                break;
        }

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response
            .WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }

}
