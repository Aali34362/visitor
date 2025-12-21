namespace Visitor.Module.IAM.Controllers.v1;

[ApiController]
[Route("v{version:apiVersion}/[module]/[controller]")]
[SwaggerTag("Role Information Controller")]
public class RoleController(IIdentityRoleAppService RoleService) : BaseController
{
    private readonly IIdentityRoleAppService _RoleService = RoleService;

    [HttpGet]
    [SwaggerOperation(
        Summary = "Get a list of Role",
        Description = "Use the endpoint to retrieve a list of Role.",
        OperationId = "GetRole",
        Tags = ["Role"])]
    [SwaggerResponse(200, "List of Role", type: typeof(PaginatedList<IdentityRoleList>))]
    [SwaggerResponse(404, "Role not found")]
    [ProducesResponseType(typeof(PaginatedList<IdentityRoleList>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetAllAsync([FromQuery] GetRoleListQuery query)
    {
        var result = await _RoleService.GetAllAsync(query);
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
        Summary = "Get Role by Id",
        Description = "Retrieve details of a single Role by its Id.",
        OperationId = "GetRoleById",
        Tags = ["Role"])]
    [SwaggerResponse(200, "Detail of Role", type: typeof(IdentityRoleDetail))]
    [SwaggerResponse(404, "Role not found")]
    [ProducesResponseType(typeof(IdentityRoleDetail), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetByIdAsync([FromRoute][Required] Guid id)
    {
        var result = await _RoleService.GetByIdAsync(new() { Id = id });
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
        Summary = "Add a new Role",
        Description = "Use the endpoint to add a new Role.",
        OperationId = "CreateRole",
        Tags = ["Role"])]
    [SwaggerResponse(200, "The Role has been successfully added", type: typeof(Result<bool>))]
    [SwaggerResponse(400, "Invalid Role input")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateAsync([FromBody] CreateRoleCommand command)
    {
        var result = await _RoleService.CreateAsync(command);
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
        Summary = "Update an existing Role",
        Description = "Use the endpoint to update an existing Role by its ID.",
        OperationId = "UpdateRole",
        Tags = ["Role"])]
    [SwaggerResponse(200, "The Role has been successfully updated", type: typeof(Result<bool>))]
    [SwaggerResponse(400, "Invalid Role input")]
    [SwaggerResponse(404, "Role not found")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdateAsync([FromRoute][Required] Guid id, [FromBody] UpdateRoleCommand command)
    {
        command.Id = id;
        var result = await _RoleService.UpdateAsync(command);
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
        Summary = "Delete an existing Role",
        Description = "Use the endpoint to delete an existing Role by its ID.",
        OperationId = "DeleteRole",
        Tags = ["Role"])]
    [SwaggerResponse(200, "The Role has been successfully deleted", type: typeof(Result<bool>))]
    [SwaggerResponse(404, "Role not found")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteAsync([FromRoute][Required] Guid id)
    {
        var result = await _RoleService.DeleteAsync(new() { Id = id });
        if (!result.IsSuccess)
        {
            if (result.Error?.Type == ErrorTypeValues.NotFound)
                return NotFound(result.Error);
            return BadRequest(result.Error);
        }
        return Ok(result.Value);
    }
}
