namespace Visitor.Core.Domain.Exceptions;

public class HandlerNotFoundException : InvalidOperationException
{
    public HandlerNotFoundException(Type handlerType, Type requestType)
        : base($"No handler '{handlerType.FullName}' is registered for request '{requestType.FullName}'.") { }
}
