namespace Visitor.Module.DMS.Domain.Models;

public class DocumentCategory : BaseModel
{
    public string Name { get; set; } = null!;
    public string Tags { get; set; }
}
