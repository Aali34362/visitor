namespace Visitor.Core.DesignPatterns.MediatRPattern;

public interface IMediatRDispatcher
{
    Task CommandAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default) where TCommand : ICommand;
    Task<TResult> CommandAsync<TCommand, TResult>(TCommand command, CancellationToken cancellationToken = default) where TCommand : ICommand<TResult>;
    Task<TResult> QueryAsync<TQuery, TResult>(TQuery query, CancellationToken ct = default) where TQuery : IQuery<TResult>;
}
