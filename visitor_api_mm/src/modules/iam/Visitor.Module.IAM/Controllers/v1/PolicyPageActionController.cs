namespace Visitor.Module.IAM.Controllers.v1;

[ApiController]
[Route("v{version:apiVersion}/[module]/[controller]")]
[SwaggerTag("PolicyPageAction Information Controller")]
public class PolicyPageActionController(IIdentityPolicyPageActionAppService PolicyPageActionMappingService) : BaseController
{
    private readonly IIdentityPolicyPageActionAppService _PolicyPageActionMappingService = PolicyPageActionMappingService;

    [HttpGet]
    [SwaggerOperation(
        Summary = "Get a list of PolicyPageAction",
        Description = "Use the endpoint to retrieve a list of PolicyPageAction.",
        OperationId = "GetPolicyPageAction",
        Tags = ["PolicyPageAction"])]
    [SwaggerResponse(200, "List of PolicyPageAction", type: typeof(PaginatedList<IdentityPolicyPageActionMappingList>))]
    [SwaggerResponse(404, "PolicyPageAction not found")]
    [ProducesResponseType(typeof(PaginatedList<IdentityPolicyPageActionMappingList>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetAllAsync([FromQuery] GetPolicyPageActionMappingListQuery query)
    {
        var result = await _PolicyPageActionMappingService.GetAllAsync(query);
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
        Summary = "Get PolicyPageAction by Id",
        Description = "Retrieve details of a single PolicyPageAction by its Id.",
        OperationId = "GetPolicyPageActionById",
        Tags = ["PolicyPageAction"])]
    [SwaggerResponse(200, "Detail of PolicyPageAction", type: typeof(IdentityPolicyPageActionMappingDetail))] 
    [SwaggerResponse(404, "PolicyPageAction not found")]
    [ProducesResponseType(typeof(IdentityPolicyPageActionMappingDetail), StatusCodes.Status200OK)] 
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetByIdAsync([FromRoute][Required] Guid id)
    {
        var result = await _PolicyPageActionMappingService.GetByIdAsync(new() { Id = id });
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
        Summary = "Add a new PolicyPageAction",
        Description = "Use the endpoint to add a new PolicyPageAction.",
        OperationId = "CreatePolicyPageAction",
        Tags = ["PolicyPageAction"])]
    [SwaggerResponse(200, "The PolicyPageAction has been successfully added", type: typeof(Result<bool>))]
    [SwaggerResponse(400, "Invalid PolicyPageAction input")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateAsync([FromBody] CreatePolicyPageActionMappingCommand command)
    {
        var result = await _PolicyPageActionMappingService.CreateAsync(command);
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
        Summary = "Update an existing PolicyPageAction",
        Description = "Use the endpoint to update an existing PolicyPageAction by its ID.",
        OperationId = "UpdatePolicyPageAction",
        Tags = ["PolicyPageAction"])]
    [SwaggerResponse(200, "The PolicyPageAction has been successfully updated", type: typeof(Result<bool>))]
    [SwaggerResponse(400, "Invalid PolicyPageAction input")]
    [SwaggerResponse(404, "PolicyPageAction not found")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdateAsync([FromRoute][Required] Guid id, [FromBody] UpdatePolicyPageActionMappingCommand command)
    {
        command.Id = id;
        var result = await _PolicyPageActionMappingService.UpdateAsync(command);
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
        Summary = "Delete an existing PolicyPageAction",
        Description = "Use the endpoint to delete an existing PolicyPageAction by its ID.",
        OperationId = "DeletePolicyPageAction",
        Tags = ["PolicyPageAction"])]
    [SwaggerResponse(200, "The PolicyPageAction has been successfully deleted", type: typeof(Result<bool>))]
    [SwaggerResponse(404, "PolicyPageAction not found")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteAsync([FromRoute][Required] Guid id)
    {
        var result = await _PolicyPageActionMappingService.DeleteAsync(new() { Id = id });
        if (!result.IsSuccess)
        {
            if (result.Error?.Type == ErrorTypeValues.NotFound)
                return NotFound(result.Error);
            return BadRequest(result.Error);
        }
        return Ok(result.Value);
    }
}
