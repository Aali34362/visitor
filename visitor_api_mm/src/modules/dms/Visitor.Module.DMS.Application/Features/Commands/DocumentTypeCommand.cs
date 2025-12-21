namespace Visitor.Module.DMS.Application.Features.Commands;

public abstract record DocumentTypeCommand(Guid Category_Id, string Tags);

#region Create Document Type Command
public record CreateDocumentTypeCommand(string Name, Guid Category_Id, string Tags) 
    : DocumentTypeCommand(Category_Id, Tags), 
    ICommand<Result<bool>>;

public class CreateDocumentTypeCommandHandler : ICommandHandler<CreateDocumentTypeCommand, Result<bool>>
{
    private readonly IDomainEventSink _events;
    private readonly CorrelationService _corr;
    private readonly ILogger<CreateDocumentTypeCommandHandler> _logger;
    public CreateDocumentTypeCommandHandler(IDomainEventSink events, CorrelationService corr, ILogger<CreateDocumentTypeCommandHandler> logger)
    {
        _logger = logger;
        _events = events;
        _corr = corr;
    }
    public Task<Result<bool>> HandleAsync(CreateDocumentTypeCommand command, CancellationToken ct)
    {
        //Validation for Duplicate Document Type can be added here

        var documentType = new DocumentType
        {
            Name = command.Name,
            Category_Id = command.Category_Id,
            Tags = command.Tags
        };

        _logger.LogInformation("Document Type Created: {@DocumentType}", documentType);
        _events.Raise(new DocumentTypeCreatedEvent(documentType, _corr.GetCorrelationId()));
        return Task.FromResult( Result<bool>.Success(true));
    }
}

public class CreateDocumentTypeCommandValidator : AbstractValidator<CreateDocumentTypeCommand>
{
    public CreateDocumentTypeCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");
        RuleFor(x => x.Category_Id)
            .NotEmpty().WithMessage("Category ID is required.");
    }
}
#endregion

#region Update Document Type Command
public record UpdateDocumentTypeCommand(Guid Id, Guid Category_Id, string Tags) 
    : DocumentTypeCommand(Category_Id, Tags),
    ICommand<Result<bool>>;

public class UpdateDocumentTypeCommandHandler : ICommandHandler<UpdateDocumentTypeCommand, Result<bool>>
{
    private readonly ILogger<UpdateDocumentTypeCommandHandler> _logger;
    public UpdateDocumentTypeCommandHandler(ILogger<UpdateDocumentTypeCommandHandler> logger)
    {
        _logger = logger;
    }
    public async Task<Result<bool>> HandleAsync(UpdateDocumentTypeCommand command, CancellationToken ct)
    {
        //Validation for Duplicate Document Type can be added here

        var documentType = new DocumentType
        {
            Id = command.Id,
            Category_Id = command.Category_Id,
            Tags = command.Tags
        };

        _logger.LogInformation("Document Type Updated: {@DocumentType}", documentType);

        return Result<bool>.Success(true);
    }
}

public class UpdateDocumentTypeCommandValidator : AbstractValidator<UpdateDocumentTypeCommand>
{
    public UpdateDocumentTypeCommandValidator()
    {
        RuleFor(x => x.Category_Id)
            .NotEmpty().WithMessage("Category ID is required.");
    }
}
#endregion

#region Delete Document Type Command
public record DeleteDocumentTypeCommand(Guid Id)
    : ICommand<Result<bool>>;

public class DeleteDocumentTypeCommandHandler : ICommandHandler<DeleteDocumentTypeCommand, Result<bool>>
{
    private readonly ILogger<DeleteDocumentTypeCommandHandler> _logger;
    public DeleteDocumentTypeCommandHandler(ILogger<DeleteDocumentTypeCommandHandler> logger)
    {
        _logger = logger;
    }
    public async Task<Result<bool>> HandleAsync(DeleteDocumentTypeCommand command, CancellationToken ct)
    {
        //Validation for existence of Document Type can be added here
        _logger.LogInformation("Document Type Deleted with Id: {Id}", command.Id);
        return Result<bool>.Success(true);
    }
}
#endregion
