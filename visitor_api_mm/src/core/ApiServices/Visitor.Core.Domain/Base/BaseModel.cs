using System.ComponentModel.DataAnnotations;

namespace Visitor.Core.Domain.Base;

public class BaseModel
{
    public Guid id { get; set; } = Guid.NewGuid();
    public string created_By { get; set; }
    public DateTime created_At { get; set; } = DateTime.UtcNow;
    public string updated_By { get; set; }
    public DateTime updated_At { get; set; } = DateTime.UtcNow;
    public int act_Ind { get; set; } = 1; // Active indicator, 1 for active, 0 for inactive
    public bool is_Deleted { get; set; } = false; // Soft delete indicator
    ////[ConcurrencyCheck]
    public int Version { get; set; } = 1; // Versioning for optimistic concurrency control
}
