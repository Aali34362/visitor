namespace Visitor.Module.DMS.Domain.Responses.Lists;

public class DocumentTypeList : BaseResponse
{
    public string Name { get; set; } = null!;
    public string Category_Nm { get; set; }
    public Dictionary<string, string> Tags { get; set; }
}
