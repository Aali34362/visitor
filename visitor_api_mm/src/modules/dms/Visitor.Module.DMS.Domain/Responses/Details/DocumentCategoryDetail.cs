namespace Visitor.Module.DMS.Domain.Responses.Details;

public class DocumentCategoryDetail : BaseResponse
{
    public string Name { get; set; } = null!;
    public Dictionary<string, string> Tags { get; set; }
}
