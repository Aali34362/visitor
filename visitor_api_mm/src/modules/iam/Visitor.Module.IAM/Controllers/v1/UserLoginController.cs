namespace Visitor.Module.IAM.Controllers.v1;

[ApiController]
[Route("v{version:apiVersion}/[module]/[controller]")]
[SwaggerTag("UserLogin Information Controller")]
public class UserLoginController(IIdentityUserLoginAppService UserLoginService) : BaseController
{
    private readonly IIdentityUserLoginAppService _UserLoginService = UserLoginService;

    [HttpGet]
    [SwaggerOperation(
        Summary = "Get a list of UserLogin",
        Description = "Use the endpoint to retrieve a list of UserLogin.",
        OperationId = "GetUserLogin",
        Tags = ["UserLogin"])]
    [SwaggerResponse(200, "List of UserLogin", type: typeof(PaginatedList<IdentityUserLoginList>))]
    [SwaggerResponse(404, "UserLogin not found")]
    [ProducesResponseType(typeof(PaginatedList<IdentityUserLoginList>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetAllAsync([FromQuery] GetUserLoginListQuery query)
    {
        var result = await _UserLoginService.GetAllAsync(query);
        if (!result.IsSuccess)
        {
            if (result.Error?.Type == ErrorTypeValues.NotFound)
                return NotFound(result.Error);
            return BadRequest(result.Error);
        }
        return Ok(result.Value);
    }

    [HttpPost]
    [SwaggerOperation(
         Summary = "Add a new UserLogin",
         Description = "Use the endpoint to add a new UserLogin.",
         OperationId = "CreateUserLogin",
         Tags = ["UserLogin"])]
    [SwaggerResponse(200, "The UserLogin has been successfully added", type: typeof(Result<bool>))]
    [SwaggerResponse(400, "Invalid UserLogin input")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateAsync([FromBody] CreateUserLoginCommand command)
    {
        var result = await _UserLoginService.CreateAsync(command);
        if (!result.IsSuccess)
        {
            if (result.Error?.Type == ErrorTypeValues.NotFound)
                return NotFound(result.Error);
            return BadRequest(result.Error);
        }
        return Ok(result.Value);
    }

    [HttpPut("{id}")]
    [SwaggerOperation(
       Summary = "Update an existing UserLogin",
       Description = "Use the endpoint to update an existing UserLogin by its ID.",
       OperationId = "UpdateUserLogin",
       Tags = ["UserLogin"])]
    [SwaggerResponse(200, "The UserLogin has been successfully updated", type: typeof(Result<bool>))]
    [SwaggerResponse(400, "Invalid UserLogin input")]
    [SwaggerResponse(404, "UserLogin not found")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdateAsync([FromRoute][Required] Guid id, [FromBody] UpdateUserLoginCommand command)
    {
        command.Id = id;
        var result = await _UserLoginService.UpdateAsync(command);
        if (!result.IsSuccess)
        {
            if (result.Error?.Type == ErrorTypeValues.NotFound)
                return NotFound(result.Error);
            return BadRequest(result.Error);
        }
        return Ok(result.Value);
    }
}
