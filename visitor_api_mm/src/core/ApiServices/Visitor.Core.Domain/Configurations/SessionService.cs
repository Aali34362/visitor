namespace Visitor.Core.Domain.Configurations;

public class SessionService
{
    private static readonly AsyncLocal<Guid> _sessionId = new();
    public Guid GetSessionId() => _sessionId.Value;
    public void SetSessionId(Guid sessionId) => _sessionId.Value = sessionId;
}
