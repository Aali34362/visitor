namespace Visitor.Module.DMS.Domain.Responses.Lists;

public class DocumentCategoryList : BaseResponse
{
    public string Name { get; set; } = null!;
    public Dictionary<string, string> Tags { get; set; }
}
