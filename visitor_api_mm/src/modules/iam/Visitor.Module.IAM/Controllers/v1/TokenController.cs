using Microsoft.AspNetCore.Authorization;

namespace Visitor.Module.IAM.Controllers.v1;

[ApiController]
[Route("v{version:apiVersion}/[module]/[controller]")]
public class TokenController(ITokenGenerationAppService tokenService) : BaseController
{
    private readonly ITokenGenerationAppService _tokenService = tokenService;

    [HttpPost]
    [Consumes("application/x-www-form-urlencoded")]
    [AllowAnonymous]
    public async Task<ActionResult> CreateAsync([FromForm] GenerateTokenCommand command)
    {
        var response = await _tokenService.GenerateTokenAsync(command);
        if (response.Value is null)
            return Unauthorized();
        return new OkObjectResult(response.Value);
    }
}
