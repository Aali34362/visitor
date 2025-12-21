using Microsoft.AspNetCore.Mvc.Filters;

namespace Visitor.Core.WebServices.Filters;

public class PlainTextResultFilter : IAsyncResultFilter
{
    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        if (context.Result is ObjectResult objectResult && objectResult.Value != null)
        {
            var type = objectResult.Value.GetType();

            // Handle primitives, Guid, string, decimal, etc.
            if (type.IsPrimitive || type == typeof(string) || type == typeof(Guid) || type == typeof(decimal))
            {
                var value = objectResult.Value.ToString();
                context.Result = new ContentResult
                {
                    Content = value,
                    ContentType = "text/plain",
                    StatusCode = objectResult.StatusCode ?? 200
                };
            }
        }

        await next();
    }
}
