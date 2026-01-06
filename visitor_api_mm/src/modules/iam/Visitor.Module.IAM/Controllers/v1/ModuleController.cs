using Visitor.Core.DesignPatterns.MediatRPattern;

namespace Visitor.Module.IAM.Controllers.v1;

[ApiController]
[Route("v{version:apiVersion}/[module]/[controller]")]
[SwaggerTag("Module Information Controller")]
public class ModuleController(IIdentityModuleAppService moduleService) : BaseController
{
    private readonly IIdentityModuleAppService _moduleService = moduleService;

    [HttpGet]
    [SwaggerOperation(
    Summary = "Get a list of Modules",
    Description = "Retrieve a paginated list of Modules.",
    OperationId = "GetAllModules",
    Tags = ["Module"])]
    [SwaggerResponse(200, "List of Module", type: typeof(PaginatedList<IdentityModuleList>))]
    [SwaggerResponse(404, "Module not found")]
    [ProducesResponseType(typeof(PaginatedList<IdentityModuleList>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllAsync([FromQuery] GetModuleListQuery query)
    {
        var result = await _moduleService.GetAllAsync(query);
        return HandleResult(result);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(
    Summary = "Get Module by Id",
    Description = "Retrieve details of a single Module by its Id.",
    OperationId = "GetModuleById",
    Tags = ["Module"])]
    [SwaggerResponse(200, "Detail of Module", type: typeof(IdentityModuleDetail))]
    [SwaggerResponse(404, "Module not found")]
    [ProducesResponseType(typeof(IdentityModuleDetail), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync([FromRoute][Required] Guid id)
    {
        var result = await _moduleService.GetByIdAsync(new() { Id = id });
        return HandleResult(result);
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Add a new Module",
        Description = "Use the endpoint to add a new Module.",
        OperationId = "AddModule",
        Tags = new[] { "Module" })]
    [SwaggerResponse(204, "The Module has been successfully added")]
    [SwaggerResponse(400, "Invalid Module input")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateModuleCommand command)
    {
        var result = await _moduleService.CreateAsync(command);
        return HandleResult(result);
    }

    [HttpPut("{id}")]
    [SwaggerOperation(
        Summary = "Update an existing Module",
        Description = "Use the endpoint to update an existing Module by its ID.",
        OperationId = "UpdateModule",
        Tags = new[] { "Module" })]
    [SwaggerResponse(204, "The Module has been successfully updated")]
    [SwaggerResponse(400, "Invalid Module input")]
    [SwaggerResponse(404, "Module not found")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAsync([FromRoute][Required] Guid id, [FromBody] UpdateModuleCommand command)
    {
        command.Id = id;
        var result = await _moduleService.UpdateAsync(command);
        return HandleResult(result);
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(
        Summary = "Delete an existing Module",
        Description = "Use the endpoint to delete an existing Module by its ID.",
        OperationId = "DeleteModule",
        Tags = new[] { "Module" })]
    [SwaggerResponse(204, "The Module has been successfully deleted")]
    [SwaggerResponse(404, "Module not found")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync([FromRoute][Required] Guid id)
    {
        var result = await _moduleService.DeleteAsync(new() { Id = id });
        return HandleResult(result);
    }
}
