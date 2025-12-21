namespace Visitor.Module.DMS.Domain.Events;

public sealed record DocumentTypeCreatedEvent(DocumentType documentType, Guid correlation_Id) : IDomainEvent
{
    public DateTimeOffset OccurredOn { get; } = DateTimeOffset.UtcNow;

    Guid IDomainEvent.CorrelationId => correlation_Id;
}