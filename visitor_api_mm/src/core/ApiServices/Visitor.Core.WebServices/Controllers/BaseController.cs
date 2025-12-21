using Microsoft.AspNetCore.Mvc.Filters;
using System.Net.Mime;

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
}
