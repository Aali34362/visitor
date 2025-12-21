namespace Visitor.Module.DMS.Domain.Models;

public class DocumentType : BaseModel
{
    public string Name { get; set; } = null!;
    public Guid Category_Id { get; set; }
    public string Tags { get; set; }
}
