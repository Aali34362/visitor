namespace Visitor.Module.DMS.Application.Features.Queries;

#region Get Document Type By Id Query
public record GetDocumentTypeByIdQuery(Guid Id) : IQuery<Result<DocumentTypeDetail>>;

public class GetDocumentTypeByIdQueryHandler : IQueryHandler<GetDocumentTypeByIdQuery, Result<DocumentTypeDetail>>
{
    private readonly ILogger<GetDocumentTypeByIdQueryHandler> _logger;
    public GetDocumentTypeByIdQueryHandler(ILogger<GetDocumentTypeByIdQueryHandler> logger)
    {
        _logger = logger;
    }
    public async Task<Result<DocumentTypeDetail>> HandleAsync(GetDocumentTypeByIdQuery query, CancellationToken ct)
    {
        // Simulate fetching from a data source
        var documentType = new DocumentTypeDetail
        {
            Id = query.Id,
            Name = "Sample Document Type",
            Category_Nm = "Sample Category",
            Tags = new Dictionary<string, string> { { "Key1", "Value1" }, { "Key2", "Value2" } }
        };
        _logger.LogInformation("Fetched Document Type: {@DocumentType}", documentType);
        return Result<DocumentTypeDetail>.Success(documentType);
    }
}

#endregion

#region Get All Document Types Query

public record GetAllDocumentTypesQuery(string Name, string Category_Nm) : IQuery<Result<PaginatedList<DocumentTypeList>>>;

public class GetAllDocumentTypesQueryHandler : IQueryHandler<GetAllDocumentTypesQuery, Result<PaginatedList<DocumentTypeList>>>
{
    private readonly ILogger<GetAllDocumentTypesQueryHandler> _logger;
    public GetAllDocumentTypesQueryHandler(ILogger<GetAllDocumentTypesQueryHandler> logger)
    {
        _logger = logger;
    }
    public async Task<Result<PaginatedList<DocumentTypeList>>> HandleAsync(GetAllDocumentTypesQuery query, CancellationToken ct)
    {
        // Simulate fetching from a data source
        var documentTypes = new List<DocumentTypeList>
        {
            new DocumentTypeList
            {
                Id = Guid.NewGuid(),
                Name = "Sample Document Type 1",
                Category_Nm = "Sample Category 1",
                Tags = new Dictionary<string, string> { { "Key1", "Value1" } }
            },
            new DocumentTypeList
            {
                Id = Guid.NewGuid(),
                Name = "Sample Document Type 2",
                Category_Nm = "Sample Category 2",
                Tags = new Dictionary<string, string> { { "Key2", "Value2" } }
            }
        };
        var paginatedList = new PaginatedList<DocumentTypeList>();
        paginatedList.Items = documentTypes;
        paginatedList.TotalItems = documentTypes.Count;
        paginatedList.PageSize = 10;
        paginatedList.PageNumber = 1;
        paginatedList.Start = 1;
        paginatedList.End = documentTypes.Count;

        _logger.LogInformation("Fetched Document Types: {@DocumentTypes}", paginatedList);
        return Result<PaginatedList<DocumentTypeList>>.Success(paginatedList);
    }
}

#endregion
