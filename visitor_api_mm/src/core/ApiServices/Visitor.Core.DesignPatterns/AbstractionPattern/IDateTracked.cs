namespace Visitor.Core.DesignPatterns.AbstractionPattern;

public interface IDateTracked { DateTimeOffset CreatedAt { get; } DateTimeOffset? UpdatedAt { get; } }
