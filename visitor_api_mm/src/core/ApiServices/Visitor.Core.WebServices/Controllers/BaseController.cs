using Microsoft.AspNetCore.Mvc.Filters;
using System.Net.Mime;
using Visitor.Core.DesignPatterns.ResultPattern;

namespace Visitor.Core.ApiServices.Controllers;

[Authorize]
[Route("v{version:apiVersion}")]
[ApiVersion("1")]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
[ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status403Forbidden)]
[ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
public abstract class BaseController : Controller
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);
    }

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        base.OnActionExecuted(context);
    }

    protected IActionResult HandleResult<T>(Result<T> result)
    {
        if (!result.IsSuccess)
        {
            if (result.Error?.Type == ErrorTypeValues.NotFound)
                return NotFound(result.Error);
            return BadRequest(result.Error);
        }
        return Ok(result.Value);
    }
}
