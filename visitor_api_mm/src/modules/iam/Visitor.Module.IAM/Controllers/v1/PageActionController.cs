namespace Visitor.Module.IAM.Controllers.v1;

[ApiController]
[Route("v{version:apiVersion}/[module]/[controller]")]
[SwaggerTag("PageAction Information Controller")]
public class PageActionController(IIdentityPageActionAppService PageActionService) : BaseController
{
    private readonly IIdentityPageActionAppService _PageActionService = PageActionService;

    [HttpGet]
    [SwaggerOperation(
        Summary = "Get a list of PageAction",
        Description = "Use the endpoint to retrieve a list of PageAction.",
        OperationId = "GetPageAction",
        Tags = ["PageAction"])]
    [SwaggerResponse(200, "List of PageAction", type: typeof(PaginatedList<IdentityPageActionList>))]
    [SwaggerResponse(404, "PageAction not found")]
    [ProducesResponseType(typeof(PaginatedList<IdentityPageActionList>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetAllAsync([FromQuery] GetPageActionListQuery query)
    {
        var result = await _PageActionService.GetAllAsync(query);
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
        Summary = "Get PageAction by Id",
        Description = "Retrieve details of a single PageAction by its Id.",
        OperationId = "GetPageActionById",
        Tags = ["PageAction"])]
    [SwaggerResponse(200, "Detail of PageAction", type: typeof(IdentityPageActionDetail))]
    [SwaggerResponse(404, "PageAction not found")]
    [ProducesResponseType(typeof(IdentityPageActionDetail), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetByIdAsync([FromRoute][Required] Guid id)
    {
        var result = await _PageActionService.GetByIdAsync(new() { Id = id });
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
        Summary = "Add a new PageAction",
        Description = "Use the endpoint to add a new PageAction.",
        OperationId = "CreatePageAction",
        Tags = ["PageAction"])]
    [SwaggerResponse(200, "The PageAction has been successfully added", type: typeof(Result<bool>))]
    [SwaggerResponse(400, "Invalid PageAction input")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateAsync([FromBody] CreatePageActionCommand command)
    {
        var result = await _PageActionService.CreateAsync(command);
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
        Summary = "Update an existing PageAction",
        Description = "Use the endpoint to update an existing PageAction by its ID.",
        OperationId = "UpdatePageAction",
        Tags = ["PageAction"])]
    [SwaggerResponse(200, "The PageAction has been successfully updated", type: typeof(Result<bool>))]
    [SwaggerResponse(400, "Invalid PageAction input")]
    [SwaggerResponse(404, "PageAction not found")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdateAsync([FromRoute][Required] Guid id, [FromBody] UpdatePageActionCommand command)
    {
        command.Id = id;
        var result = await _PageActionService.UpdateAsync(command);
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
        Summary = "Delete an existing PageAction",
        Description = "Use the endpoint to delete an existing PageAction by its ID.",
        OperationId = "DeletePageAction",
        Tags = ["PageAction"])]
    [SwaggerResponse(200, "The PageAction has been successfully deleted", type: typeof(Result<bool>))]
    [SwaggerResponse(404, "PageAction not found")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteAsync([FromRoute][Required] Guid id)
    {
        var result = await _PageActionService.DeleteAsync(new() { Id = id });
        if (!result.IsSuccess)
        {
            if (result.Error?.Type == ErrorTypeValues.NotFound)
                return NotFound(result.Error);
            return BadRequest(result.Error);
        }
        return Ok(result.Value);
    }
}
