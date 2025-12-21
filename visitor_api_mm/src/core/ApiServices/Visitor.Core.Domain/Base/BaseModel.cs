using System.ComponentModel.DataAnnotations;

namespace Visitor.Core.Domain.Base;

public class BaseModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public int Act_Ind { get; set; } = 1; // Active indicator, 1 for active, 0 for inactive
    public bool IsDeleted { get; set; } = false; // Soft delete indicator
    public string CreatedBy { get; set; }
    public string UpdatedBy { get; set; }
    ////[ConcurrencyCheck]
    public int Version { get; set; } = 1; // Versioning for optimistic concurrency control
}
