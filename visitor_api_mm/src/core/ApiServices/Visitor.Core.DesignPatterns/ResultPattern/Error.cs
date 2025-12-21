using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Visitor.Core.DesignPatterns.ResultPattern;

public sealed record Error(string Code, string Message);

public class ErrorDetail
{
    public string TraceId { get; init; }
    public string Type { get; set; } = "about:blank";
    public string Title { get; set; } = "Error";
    public int Status { get; set; } = 400;
    public string Detail { get; init; }
    public string Instance { get; init; }
    public Dictionary<string, string[]> Errors { get; set; } = null!;
    
    public string Code { get; init; }

    public static ErrorDetail Domain(Dictionary<string, string[]> errors)
    {
        return new ErrorDetail
        {
            Type = "DomainValidationError",
            Title = "BadRequest",
            Status = 400,
            Errors = errors
        };
    }
    public static ErrorDetail Business(string errorMessage, string propertyName)
    {
        return new ErrorDetail
        {
            Type = "BusinessLogicError",
            Title = "BadRequest",
            Status = 400,
            Errors = new Dictionary<string, string[]>
        {
            { propertyName, new[] { errorMessage } }
        }
        };
    }
    public static ErrorDetail NotFound(string errorMessage, string propertyName)
    {
        return new ErrorDetail
        {
            Type = "NotFoundError",
            Title = "NotFound",
            Status = 404,
            Errors = new Dictionary<string, string[]>
        {
            { propertyName, new[] { errorMessage } }
        }
        };
    }

    public static ErrorDetail FromModelState(ModelStateDictionary modelState)
    {
        var errors = modelState
            .Where(kvp => kvp.Value?.Errors.Count > 0)
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
            );

        return Domain(errors);
    }
}
