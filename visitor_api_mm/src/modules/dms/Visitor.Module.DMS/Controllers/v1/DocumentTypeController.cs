namespace Visitor.Module.DMS.Controllers.v1;

[ApiController]
[Route("v{version:apiVersion}/[module]/[controller]")]
[SwaggerTag("Document Type Information Controller")]
public class DocumentTypeController(IMediatRDispatcher mediatR) : BaseController
{
    private readonly IMediatRDispatcher _mediatR = mediatR;

    [HttpGet]
    [SwaggerOperation(
        Summary = "Get a list of Document Type",
        Description = "Use the endpoint to retrieve a list of Document Type.",
        OperationId = "GetDocumentType",
        Tags = ["DocumentType"])]
    [SwaggerResponse(200, "List of DocumentType", type: typeof(PaginatedList<DocumentTypeList>))]
    [SwaggerResponse(404, "DocumentType not found")]
    [ProducesResponseType(typeof(PaginatedList<DocumentTypeList>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetAllAsync([FromQuery] GetAllDocumentTypesQuery query, CancellationToken cancellationToken)
    {
        var result = await _mediatR.QueryAsync<GetAllDocumentTypesQuery, Result<PaginatedList<DocumentTypeList>>>(query, cancellationToken);
        if (!result.IsSuccess)
            return result.Error?.Type == ErrorTypeValues.NotFound
                ? NotFound(result.Error)
                : BadRequest(result.Error);
        return Ok(result.Value);
    }


    [HttpGet("{id}")]
    [SwaggerOperation(
        Summary = "Get Document Type by Id",
        Description = "Retrieve details of a single Document Type by its Id.",
        OperationId = "GetDocumentTypeById",
        Tags = ["DocumentType"])]
    [SwaggerResponse(200, "Detail of Document Type", type: typeof(DocumentTypeDetail))]
    [SwaggerResponse(404, "Document Type not found")]
    [ProducesResponseType(typeof(DocumentTypeDetail), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetByIdAsync([FromRoute][Required] Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediatR.QueryAsync<GetDocumentTypeByIdQuery, Result<DocumentTypeDetail>>(new GetDocumentTypeByIdQuery(id), cancellationToken);
        if (!result.IsSuccess)
            return result.Error?.Type == ErrorTypeValues.NotFound
                ? NotFound(result.Error)
                : BadRequest(result.Error);
        return Ok(result.Value);
    }


    [HttpPost]
    [SwaggerOperation(
         Summary = "Add a new Document Type",
         Description = "Use the endpoint to add a new Document Type.",
         OperationId = "CreateDocumentType",
         Tags = ["DocumentType"])]
    [SwaggerResponse(200, "The Document Type has been successfully added", type: typeof(Result<bool>))]
    [SwaggerResponse(400, "Invalid Document Type input")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateAsync([FromBody] CreateDocumentTypeCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediatR.CommandAsync<CreateDocumentTypeCommand, Result<bool>>(command, cancellationToken);
        if (!result.IsSuccess)
            return result.Error?.Type == ErrorTypeValues.NotFound
                ? NotFound(result.Error)
                : BadRequest(result.Error);
        return Ok(result.Value);
    }

    [HttpPut("{id}")]
    [SwaggerOperation(
        Summary = "Update an existing Document Type",
        Description = "Use the endpoint to update an existing Document Type by its ID.",
        OperationId = "UpdateDocumentType",
        Tags = ["DocumentType"])]
    [SwaggerResponse(200, "The Document Type has been successfully updated", type: typeof(Result<bool>))]
    [SwaggerResponse(400, "Invalid Document Type input")]
    [SwaggerResponse(404, "Document Type not found")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdateAsync([FromRoute][Required] Guid id, [FromBody] UpdateDocumentTypeCommand command, CancellationToken cancellationToken)
    {
        var cmd = command with { Id = id };
        var result = await _mediatR.CommandAsync<UpdateDocumentTypeCommand, Result<bool>>(cmd, cancellationToken);

        if (!result.IsSuccess)
            return result.Error?.Type == ErrorTypeValues.NotFound
                ? NotFound(result.Error)
                : BadRequest(result.Error);

        return Ok(result.Value);
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(
        Summary = "Delete an existing Document Type",
        Description = "Use the endpoint to delete an existing Document Type by its ID.",
        OperationId = "DeleteDocumentType",
        Tags = ["DocumentType"])]
    [SwaggerResponse(200, "The Document Type has been successfully deleted", type: typeof(Result<bool>))]
    [SwaggerResponse(404, "Document Type not found")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteAsync([FromRoute][Required] Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediatR.CommandAsync<DeleteDocumentTypeCommand, Result<bool>>(new DeleteDocumentTypeCommand(id), cancellationToken);
        if (!result.IsSuccess)
            return result.Error?.Type == ErrorTypeValues.NotFound
                ? NotFound(result.Error)
                : BadRequest(result.Error);
        return Ok(result.Value);
    }
}
