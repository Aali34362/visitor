namespace Visitor.Module.IAM.Application.ModelDtos;

public class GetPageGraphQuery
{
    public string Page_Nm { get; init; }
    public string Module_Nm { get; init; }
}

public class GetPageActionGraphQuery
{
    public string Name { get; init; }
    public string Page_Nm { get; init; }
    public string Module_Nm { get; init; }
}
