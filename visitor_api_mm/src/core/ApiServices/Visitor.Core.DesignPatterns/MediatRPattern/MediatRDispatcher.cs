using Visitor.Core.DesignPatterns.EventPattern;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using System.Reflection;

namespace Visitor.Core.DesignPatterns.MediatRPattern;

public class MediatRDispatcher(
    IServiceProvider serviceProvider, 
    IDomainEventCollector collector, 
    IEventBus bus) 
    : IMediatRDispatcher
{
    
    private static readonly ConcurrentDictionary<Type, MethodInfo> _handleMethodCache = new();

    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly IDomainEventCollector _collector = collector;
    private readonly IEventBus _bus = bus;

    public async Task CommandAsync<TCommand>(TCommand command, CancellationToken ct = default)
        where TCommand : ICommand
    {
        var handler = _serviceProvider.GetRequiredService<ICommandHandler<TCommand>>();
        var res = await handler.Handle(command, ct).ConfigureAwait(false);

        // publish only if success
        if (res.IsSuccess)
            await _bus.PublishManyAsync(_collector.DequeueAll(), ct).ConfigureAwait(false);
    }

    public async Task<TResult> CommandAsync<TCommand, TResult>(TCommand command, CancellationToken ct = default)
        where TCommand : ICommand<TResult>
    {
        var handler = _serviceProvider.GetRequiredService<ICommandHandler<TCommand, TResult>>();
        var result = await handler.HandleAsync(command, ct).ConfigureAwait(false);

        var shouldPublish = result is Result r ? r.IsSuccess : true;
        if (shouldPublish)
            await _bus.PublishManyAsync(_collector.DequeueAll(), ct).ConfigureAwait(false);

        return result;
    }

    public Task<TResult> QueryAsync<TQuery, TResult>(TQuery query, CancellationToken ct = default)
        where TQuery : IQuery<TResult>
        => _serviceProvider.GetRequiredService<IQueryHandler<TQuery, TResult>>().HandleAsync(query, ct);


    ////public async Task CommandAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default) where TCommand : ICommand
    ////{
    ////    var handlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());
    ////    var handler = _serviceProvider.GetService(handlerType) ?? throw new HandlerNotFoundException(handlerType, typeof(TCommand));
    ////    var method = GetHandleMethod(handlerType);
    ////    var task = (Task)method.Invoke(handler, new object[] { command!, cancellationToken })!;
    ////    await task.ConfigureAwait(false);
    ////}

    ////public async Task<TResult> CommandAsync<TCommand, TResult>(TCommand command, CancellationToken cancellationToken = default) where TCommand : ICommand<TResult>
    ////{
    ////    var handlerType = typeof(ICommandHandler<,>).MakeGenericType(typeof(TCommand), typeof(TResult));
    ////    var handler = _serviceProvider.GetService(handlerType) ?? throw new HandlerNotFoundException(handlerType, typeof(TCommand));
    ////    var method = GetHandleMethod(handlerType);
    ////    var task = (Task<TResult>)method.Invoke(handler, new object[] { command!, cancellationToken })!;
    ////    return await task.ConfigureAwait(false);
    ////}

    ////public async Task<TResult> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
    ////{
    ////    var queryType = query.GetType();
    ////    var handlerType = typeof(IQueryHandler<,>).MakeGenericType(queryType, typeof(TResult));
    ////    var handler = _serviceProvider.GetService(handlerType) ?? throw new HandlerNotFoundException(handlerType, queryType);
    ////    var method = GetHandleMethod(handlerType);
    ////    var task = (Task<TResult>)method.Invoke(handler, new object[] { query, cancellationToken })!;
    ////    return await task.ConfigureAwait(false);
    ////}

    ////private static MethodInfo GetHandleMethod(Type handlerType)
    ////{
    ////    return _handleMethodCache.GetOrAdd(handlerType, static t =>
    ////        t.GetMethod("HandleAsync", BindingFlags.Public | BindingFlags.Instance)
    ////        ?? t.GetMethod("Handle", BindingFlags.Public | BindingFlags.Instance)
    ////        ?? throw new InvalidOperationException($"No Handle/HandleAsync method found on {t.FullName}."));
    ////}
}
