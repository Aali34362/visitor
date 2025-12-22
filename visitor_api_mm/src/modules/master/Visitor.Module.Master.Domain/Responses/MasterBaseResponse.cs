namespace Visitor.Module.Master.Domain.Responses;

public abstract class MasterBaseResponse : BaseResponse
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
}
