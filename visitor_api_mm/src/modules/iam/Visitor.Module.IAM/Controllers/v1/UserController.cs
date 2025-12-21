namespace Visitor.Module.IAM.Controllers.v1;

[ApiController]
[Route("v{version:apiVersion}/[module]/[controller]")]
[SwaggerTag("User Information Controller")]
public class UserController(IIdentityUserAppService UserService) : BaseController
{
    private readonly IIdentityUserAppService _UserService = UserService;

    [HttpGet]    
    [SwaggerOperation(
        Summary = "Get a list of User",
        Description = "Use the endpoint to retrieve a list of User.",
        OperationId = "GetUser",
        Tags = ["User"])]
        [SwaggerResponse(200, "List of User", type: typeof(PaginatedList<IdentityUserList>))]
        [SwaggerResponse(404, "User not found")]     
    [ProducesResponseType(typeof(PaginatedList<IdentityUserList>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetAllAsync([FromQuery] GetUserListQuery query)
    {
        var result = await _UserService.GetAllAsync(query);
        if (!result.IsSuccess)
        {
            if (result.Error?.Type == ErrorTypeValues.NotFound)
                return NotFound(result.Error);
            return BadRequest(result.Error);
        }
        return Ok(result.Value);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(
        Summary = "Get User by Id",
        Description = "Retrieve details of a single User by its Id.",
        OperationId = "GetUserById",
        Tags = ["User"])]
    [SwaggerResponse(200, "Detail of User", type: typeof(IdentityUserDetail))]
    [SwaggerResponse(404, "User not found")]
    [ProducesResponseType(typeof(IdentityUserDetail), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetByIdAsync([FromRoute][Required] Guid id)
    {
        var result = await _UserService.GetByIdAsync(new() { Id = id });
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
        Summary = "Add a new User",
        Description = "Use the endpoint to add a new User.",
        OperationId = "CreateUser",
        Tags = ["User"])]
    [SwaggerResponse(200, "The User has been successfully added", type: typeof(Result<bool>))]
    [SwaggerResponse(400, "Invalid User input")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateAsync([FromBody] CreateUserCommand command)
    {
        var result = await _UserService.CreateAsync(command);
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
        Summary = "Update an existing User",
        Description = "Use the endpoint to update an existing User by its ID.",
        OperationId = "UpdateUser",
        Tags = ["User"])]
    [SwaggerResponse(200, "The User has been successfully updated", type: typeof(Result<bool>))]
    [SwaggerResponse(400, "Invalid User input")]
    [SwaggerResponse(404, "User not found")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdateAsync([FromRoute][Required] Guid id, [FromBody] UpdateUserCommand command)
    {
        command.Id = id;
        var result = await _UserService.UpdateAsync(command);
        if (!result.IsSuccess)
        {
            if (result.Error?.Type == ErrorTypeValues.NotFound)
                return NotFound(result.Error);
            return BadRequest(result.Error);
        }
        return Ok(result.Value);
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(
        Summary = "Delete an existing User",
        Description = "Use the endpoint to delete an existing User by its ID.",
        OperationId = "DeleteUser",
        Tags = ["User"])]
    [SwaggerResponse(200, "The User has been successfully deleted", type: typeof(Result<bool>))]
    [SwaggerResponse(404, "User not found")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteAsync([FromRoute][Required] Guid id)
    {
        var result = await _UserService.DeleteAsync(new() { Id = id });
        if (!result.IsSuccess)
        {
            if (result.Error?.Type == ErrorTypeValues.NotFound)
                return NotFound(result.Error);
            return BadRequest(result.Error);
        }
        return Ok(result.Value);
    }
}
