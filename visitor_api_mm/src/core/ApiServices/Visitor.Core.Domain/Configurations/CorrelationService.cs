namespace Visitor.Core.Domain.Configurations;

public class CorrelationService
{
    private static readonly AsyncLocal<Guid> _correlationId = new();
    public Guid GetCorrelationId() => _correlationId.Value;
    public void SetCorrelationId(Guid correlationId) => _correlationId.Value = correlationId;
}
