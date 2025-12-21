namespace Visitor.Module.IAM.Controllers.v1;

[ApiController]
[Route("v{version:apiVersion}/[module]/[controller]")]
[SwaggerTag("RolePolicy Information Controller")]
public class RolePolicyController(IIdentityRolePolicyAppService RolePolicyMappingService) : BaseController
{
    private readonly IIdentityRolePolicyAppService _RolePolicyMappingService = RolePolicyMappingService;

    [HttpGet]
    [SwaggerOperation(
        Summary = "Get a list of RolePolicy",
        Description = "Use the endpoint to retrieve a list of RolePolicy.",
        OperationId = "GetRolePolicy",
        Tags = ["RolePolicy"])]
    [SwaggerResponse(200, "List of RolePolicy", type: typeof(PaginatedList<IdentityRolePolicyMappingList>))]
    [SwaggerResponse(404, "RolePolicy not found")]
    [ProducesResponseType(typeof(PaginatedList<IdentityRolePolicyMappingList>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetAllAsync([FromQuery] GetRolePolicyMappingListQuery query)
    {
        var result = await _RolePolicyMappingService.GetAllAsync(query);
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
         Summary = "Get RolePolicy by Id",
         Description = "Retrieve details of a single RolePolicy by its Id.",
         OperationId = "GetRolePolicyById",
         Tags = ["RolePolicy"])]
    [SwaggerResponse(200, "Detail of RolePolicy", type: typeof(IdentityRolePolicyMappingDetail))] 
    [SwaggerResponse(404, "RolePolicyg not found")]
    [ProducesResponseType(typeof(IdentityRolePolicyMappingDetail), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetByIdAsync([FromRoute][Required] Guid id)
    {
        var result = await _RolePolicyMappingService.GetByIdAsync(new() { Id = id });
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
        Summary = "Add a new RolePolicy",
        Description = "Use the endpoint to add a new RolePolicy.",
        OperationId = "CreateRolePolicy",
        Tags = ["RolePolicy"])]
    [SwaggerResponse(200, "The RolePolicy has been successfully added", type: typeof(Result<bool>))]
    [SwaggerResponse(400, "Invalid RolePolicy input")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateAsync([FromBody] CreateRolePolicyMappingCommand command)
    {
        var result = await _RolePolicyMappingService.CreateAsync(command);
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
        Summary = "Update an existing RolePolicy",
        Description = "Use the endpoint to update an existing RolePolicy by its ID.",
        OperationId = "UpdateRolePolicy",
        Tags = ["RolePolicy"])]
    [SwaggerResponse(200, "The RolePolicy has been successfully updated", type: typeof(Result<bool>))]
    [SwaggerResponse(400, "Invalid RolePolicy input")]
    [SwaggerResponse(404, "RolePolicy not found")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdateAsync([FromRoute][Required] Guid id, [FromBody] UpdateRolePolicyMappingCommand command)
    {
        command.Id = id;
        var result = await _RolePolicyMappingService.UpdateAsync(command);
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
        Summary = "Delete an existing RolePolicy",
        Description = "Use the endpoint to delete an existing RolePolicy by its ID.",
        OperationId = "DeleteRolePolicy",
        Tags = ["RolePolicy"])]
    [SwaggerResponse(200, "The RolePolicy has been successfully deleted", type: typeof(Result<bool>))]
    [SwaggerResponse(404, "RolePolicy not found")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteAsync([FromRoute][Required] Guid id)
    {
        var result = await _RolePolicyMappingService.DeleteAsync(new() { Id = id });
        if (!result.IsSuccess)
        {
            if (result.Error?.Type == ErrorTypeValues.NotFound)
                return NotFound(result.Error);
            return BadRequest(result.Error);
        }
        return Ok(result.Value);
    }
}
