using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Visitor.Core.DesignPatterns.EventPattern;

public interface IEventBus
{
    Task PublishAsync<TEvent>(TEvent evt, CancellationToken ct = default) where TEvent : IDomainEvent;
    Task PublishManyAsync(IEnumerable<IDomainEvent> events, CancellationToken ct = default);
}

public sealed class EventBus : IEventBus
{
    private readonly IServiceProvider _sp;
    private readonly bool _parallel;

    public EventBus(IServiceProvider sp, bool publishInParallel = true)
        => (_sp, _parallel) = (sp, publishInParallel);

    public async Task PublishAsync<TEvent>(TEvent evt, CancellationToken ct = default) where TEvent : IDomainEvent
    {
        var handlers = _sp.GetServices<IDomainEventHandler<TEvent>>().ToArray();
        if (handlers.Length == 0) return;

        if (_parallel)
            await Task.WhenAll(handlers.Select(h => h.HandleAsync(evt, ct))).ConfigureAwait(false);
        else
            foreach (var h in handlers) await h.HandleAsync(evt, ct).ConfigureAwait(false);
    }

    public async Task PublishManyAsync(IEnumerable<IDomainEvent> events, CancellationToken ct = default)
    {
        foreach (var group in events.GroupBy(e => e.GetType()))
        {
            var t = group.Key;

            var method = typeof(EventBus)
                .GetMethod(nameof(PublishGeneric), BindingFlags.NonPublic | BindingFlags.Instance)!
                .MakeGenericMethod(t);

            var typedArray = Array.CreateInstance(t, group.Count());
            int i = 0;
            foreach (var e in group)
                typedArray.SetValue(e, i++);

            await (Task)method.Invoke(this, new object[] { typedArray, ct })!;
        }
    }

    private async Task PublishGeneric<TEvent>(IReadOnlyList<TEvent> evts, CancellationToken ct)
        where TEvent : IDomainEvent
    {
        var handlers = _sp.GetServices<IDomainEventHandler<TEvent>>().ToArray();
        if (handlers.Length == 0) return;

        if (_parallel)
            await Task.WhenAll(evts.SelectMany(e => handlers.Select(h => h.HandleAsync(e, ct)))).ConfigureAwait(false);
        else
            foreach (var e in evts)
                foreach (var h in handlers)
                    await h.HandleAsync(e, ct).ConfigureAwait(false);
    }
}
