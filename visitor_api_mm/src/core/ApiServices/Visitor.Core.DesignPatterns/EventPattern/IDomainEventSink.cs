using System.Collections.Concurrent;

namespace Visitor.Core.DesignPatterns.EventPattern;

public interface IDomainEventSink
{
    void Raise(IDomainEvent evt);
}

public interface IDomainEventCollector
{
    IReadOnlyCollection<IDomainEvent> DequeueAll();
}

public sealed class DomainEventSink : IDomainEventSink, IDomainEventCollector
{
    private readonly ConcurrentQueue<IDomainEvent> _queue = new();

    public void Raise(IDomainEvent evt) => _queue.Enqueue(evt);

    public IReadOnlyCollection<IDomainEvent> DequeueAll()
    {
        var list = new List<IDomainEvent>();
        while (_queue.TryDequeue(out var e)) list.Add(e);
        return list;
    }
}