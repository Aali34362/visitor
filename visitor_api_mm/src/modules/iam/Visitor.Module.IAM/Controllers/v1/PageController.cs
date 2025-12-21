namespace Visitor.Module.IAM.Controllers.v1;

[ApiController]
[Route("v{version:apiVersion}/[module]/[controller]")]
[SwaggerTag("Page Information Controller")]
public class PageController(IIdentityPageAppService PageService) : BaseController
{
    private readonly IIdentityPageAppService _PageService = PageService;

    [HttpGet]
    [SwaggerOperation(
        Summary = "Get a list of Page",
        Description = "Use the endpoint to retrieve a list of Page.",
        OperationId = "GetPage",
        Tags = ["Page"])]
    [SwaggerResponse(200, "List of Page", type: typeof(PaginatedList<IdentityPageList>))]
    [SwaggerResponse(404, "Page not found")]
    [ProducesResponseType(typeof(PaginatedList<IdentityPageList>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetAllAsync([FromQuery] GetPageListQuery query)
    {
        var result = await _PageService.GetAllAsync(query);
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
         Summary = "Get Page by Id",
         Description = "Retrieve details of a single Page by its Id.",
         OperationId = "GetPageById",
         Tags = ["Page"])]
    [SwaggerResponse(200, "Detail of Page", type: typeof(IdentityPageDetail))] 
    [SwaggerResponse(404, "Page not found")]
    [ProducesResponseType(typeof(IdentityPageDetail), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetByIdAsync([FromRoute][Required] Guid id)
    {
        var result = await _PageService.GetByIdAsync(new() { Id = id });
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
        Summary = "Add a new Page",
        Description = "Use the endpoint to add a new Page.",
        OperationId = "CreatePage",
        Tags = ["Page"])]
    [SwaggerResponse(200, "The Page has been successfully added", type: typeof(Result<bool>))]
    [SwaggerResponse(400, "Invalid Page input")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateAsync([FromBody] CreatePageCommand command)
    {
        var result = await _PageService.CreateAsync(command);
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
        Summary = "Update an existing Page",
        Description = "Use the endpoint to update an existing Page by its ID.",
        OperationId = "UpdatePage",
        Tags = ["Page"])]
    [SwaggerResponse(200, "The Page has been successfully updated", type: typeof(Result<bool>))]
    [SwaggerResponse(400, "Invalid Page input")]
    [SwaggerResponse(404, "Page not found")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdateAsync([FromRoute][Required] Guid id, [FromBody] UpdatePageCommand command)
    {
        command.Id = id;
        var result = await _PageService.UpdateAsync(command);
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
         Summary = "Delete an existing Page",
         Description = "Use the endpoint to delete an existing Page by its ID.",
         OperationId = "DeletePage",
         Tags = ["Page"])]
    [SwaggerResponse(200, "The Page has been successfully deleted", type: typeof(Result<bool>))]
    [SwaggerResponse(404, "Page not found")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteAsync([FromRoute][Required] Guid id)
    {
        var result = await _PageService.DeleteAsync(new() { Id = id });
        if (!result.IsSuccess)
        {
            if (result.Error?.Type == ErrorTypeValues.NotFound)
                return NotFound(result.Error);
            return BadRequest(result.Error);
        }
        return Ok(result.Value);
    }
}
