namespace Visitor.Module.DMS.Application.EventHandlers;

public sealed class DocumentTypeEventHandler(
    IDmsRepository repo, 
    ILogger<DocumentTypeCreatedEvent> logger) : IDomainEventHandler<DocumentTypeCreatedEvent>
{
    private readonly IDmsRepository _repo = repo ?? throw new ArgumentNullException(nameof(repo));
    private readonly ILogger<DocumentTypeCreatedEvent> _logger = logger;

    public async Task HandleAsync(DocumentTypeCreatedEvent evt, CancellationToken ct)
    {
        _logger.LogInformation("Handled {Event} for {Id}, corr={Corr}",
            nameof(DocumentTypeEventHandler), System.Text.Json.JsonSerializer.Serialize(evt.documentType), evt.correlation_Id);

        await Task.CompletedTask;
    }
}
