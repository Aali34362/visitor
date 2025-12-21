namespace Visitor.Module.IAM.Controllers.v1;

[ApiController]
[Route("v{version:apiVersion}/[module]/[controller]")]
[SwaggerTag("Policy Information Controller")]
public class PolicyController(IIdentityPolicyAppService PolicyService) : BaseController
{
    private readonly IIdentityPolicyAppService _PolicyService = PolicyService;

    [HttpGet]
    [SwaggerOperation(
        Summary = "Get a list of Policy",
        Description = "Use the endpoint to retrieve a list of Policy.",
        OperationId = "GetPolicy",
        Tags = ["Policy"])]
    [SwaggerResponse(200, "List of Policy", type: typeof(PaginatedList<IdentityPolicyList>))]
    [SwaggerResponse(404, "Policy not found")]
    [ProducesResponseType(typeof(PaginatedList<IdentityPolicyList>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetAllAsync([FromQuery] GetPolicyListQuery query)
    {
        var result = await _PolicyService.GetAllAsync(query);
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
        Summary = "Get Policy by Id",
        Description = "Retrieve details of a single Policy by its Id.",
        OperationId = "GetPolicyById",
        Tags = ["Policy"])]
    [SwaggerResponse(200, "Detail of Policy", type: typeof(IdentityPolicyDetail))] // adjust DTO if needed
    [SwaggerResponse(404, "Policy not found")]
    [ProducesResponseType(typeof(IdentityPolicyDetail), StatusCodes.Status200OK)] // adjust DTO if needed
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetByIdAsync([FromRoute][Required] Guid id)
    {
        var result = await _PolicyService.GetByIdAsync(new() { Id = id });
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
        Summary = "Add a new Policy",
        Description = "Use the endpoint to add a new Policy.",
        OperationId = "CreatePolicy",
        Tags = ["Policy"])]
    [SwaggerResponse(200, "The Policy has been successfully added", type: typeof(Result<bool>))]
    [SwaggerResponse(400, "Invalid Policy input")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateAsync([FromBody] CreatePolicyCommand command)
    {
        var result = await _PolicyService.CreateAsync(command);
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
        Summary = "Update an existing Policy",
        Description = "Use the endpoint to update an existing Policy by its ID.",
        OperationId = "UpdatePolicy",
        Tags = ["Policy"])]
    [SwaggerResponse(200, "The Policy has been successfully updated", type: typeof(Result<bool>))]
    [SwaggerResponse(400, "Invalid Policy input")]
    [SwaggerResponse(404, "Policy not found")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdateAsync([FromRoute][Required] Guid id, [FromBody] UpdatePolicyCommand command)
    {
        command.Id = id;
        var result = await _PolicyService.UpdateAsync(command);
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
        Summary = "Delete an existing Policy",
        Description = "Use the endpoint to delete an existing Policy by its ID.",
        OperationId = "DeletePolicy",
        Tags = ["Policy"])]
    [SwaggerResponse(200, "The Policy has been successfully deleted", type: typeof(Result<bool>))]
    [SwaggerResponse(404, "Policy not found")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteAsync([FromRoute][Required] Guid id)
    {
        var result = await _PolicyService.DeleteAsync(new() { Id = id });
        if (!result.IsSuccess)
        {
            if (result.Error?.Type == ErrorTypeValues.NotFound)
                return NotFound(result.Error);
            return BadRequest(result.Error);
        }
        return Ok(result.Value);
    }
}
