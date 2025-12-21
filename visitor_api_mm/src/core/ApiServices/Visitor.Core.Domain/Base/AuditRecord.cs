namespace Visitor.Core.Domain.Base;

public class AuditRecord
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string EntityName { get; set; } = null!;
    public Guid EntityId { get; set; }
    public string Operation { get; set; } = null!;
    public Dictionary<string, string> OldValues { get; set; } = new();
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public int Version { get; set; }
}
