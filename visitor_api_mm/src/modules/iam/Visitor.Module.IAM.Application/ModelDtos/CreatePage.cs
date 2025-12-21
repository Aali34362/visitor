namespace Visitor.Module.IAM.Application.ModelDtos;

public class CreatePageCommand : CreatePage
{
    public string Page_Nm { get; init; }
    public string Module_Nm { get; init; }
}

public class CreatePage
{
    public Guid Parent_Id { get; init; }
    public int Page_Level { get; init; }
    public string Page_Title { get; init; }
    public string Page_Url { get; init; }
    public int Page_Order { get; init; }
    public string Icon { get; init; }
}

public class UpdatePageCommand : CreatePage
{
    [JsonIgnore]
    public Guid Id { get; set; }
}

public class DeletePageCommand
{
    [JsonIgnore]
    public Guid Id { get; set; }
}

public class GetPageQuery
{
    public Guid Id { get; init; }
}

public class GetPageListQuery
{
    public string Page_Nm { get; init; }
    public int Page_Level { get; init; }
    public string Module_Nm { get; init; }
    public int index { get; set; } = 1;
    public int size { get; set; } = 10;
}


