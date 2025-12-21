namespace Visitor.Core.DesignPatterns.EventPattern;

public interface IDomainEvent
{
    DateTimeOffset OccurredOn { get; }
    Guid CorrelationId { get; }
}

public interface IDomainEventHandler<in TEvent> where TEvent : IDomainEvent
{
    Task HandleAsync(TEvent evt, CancellationToken ct);
}