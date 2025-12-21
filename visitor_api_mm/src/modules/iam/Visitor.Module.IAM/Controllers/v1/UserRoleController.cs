namespace Visitor.Module.IAM.Controllers.v1;

[ApiController]
[Route("v{version:apiVersion}/[module]/[controller]")]
[SwaggerTag("UserRole Information Controller")]
public class UserRoleController(IIdentityUserRoleAppService UserRoleMappingService) : BaseController
{
    private readonly IIdentityUserRoleAppService _UserRoleMappingService = UserRoleMappingService;

    [HttpGet]
    [SwaggerOperation(
        Summary = "Get a list of UserRole",
        Description = "Use the endpoint to retrieve a list of UserRole.",
        OperationId = "GetUserRole",
        Tags = ["UserRole"])]
    [SwaggerResponse(200, "List of UserRole", type: typeof(PaginatedList<IdentityUserRoleMappingList>))]
    [SwaggerResponse(404, "UserRole not found")]
    [ProducesResponseType(typeof(PaginatedList<IdentityUserRoleMappingList>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetAllAsync([FromQuery] GetUserRoleMappingListQuery query)
    {
        var result = await _UserRoleMappingService.GetAllAsync(query);
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
        Summary = "Get UserRole by Id",
        Description = "Retrieve details of a single UserRole by its Id.",
        OperationId = "GetUserRoleById",
        Tags = ["UserRole"])]
    [SwaggerResponse(200, "Detail of UserRole", type: typeof(IdentityUserRoleMappingDetail))]
    [SwaggerResponse(404, "UserRole not found")]
    [ProducesResponseType(typeof(IdentityUserRoleMappingDetail), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetByIdAsync([FromRoute][Required] Guid id)
    {
        var result = await _UserRoleMappingService.GetByIdAsync(new() { Id = id });
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
        Summary = "Add a new UserRole",
        Description = "Use the endpoint to add a new UserRole.",
        OperationId = "CreateUserRole",
        Tags = ["UserRole"])]
    [SwaggerResponse(200, "The UserRole has been successfully added", type: typeof(Result<bool>))]
    [SwaggerResponse(400, "Invalid UserRole input")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateAsync([FromBody] CreateUserRoleMappingCommand command)
    {
        var result = await _UserRoleMappingService.CreateAsync(command);
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
        Summary = "Update an existing UserRole",
        Description = "Use the endpoint to update an existing UserRole by its ID.",
        OperationId = "UpdateUserRole",
        Tags = ["UserRole"])]
    [SwaggerResponse(200, "The UserRole has been successfully updated", type: typeof(Result<bool>))]
    [SwaggerResponse(400, "Invalid UserRole input")]
    [SwaggerResponse(404, "UserRole not found")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdateAsync([FromRoute][Required] Guid id, [FromBody] UpdateUserRoleMappingCommand command)
    {
        command.Id = id;
        var result = await _UserRoleMappingService.UpdateAsync(command);
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
        Summary = "Delete an existing UserRole",
        Description = "Use the endpoint to delete an existing UserRole by its ID.",
        OperationId = "DeleteUserRole",
        Tags = ["UserRole"])]
    [SwaggerResponse(200, "The UserRole has been successfully deleted", type: typeof(Result<bool>))]
    [SwaggerResponse(404, "UserRole not found")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteAsync([FromRoute][Required] Guid id)
    {
        var result = await _UserRoleMappingService.DeleteAsync(new() { Id = id });
        if (!result.IsSuccess)
        {
            if (result.Error?.Type == ErrorTypeValues.NotFound)
                return NotFound(result.Error);
            return BadRequest(result.Error);
        }
        return Ok(result.Value);
    }
}
