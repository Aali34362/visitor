namespace Visitor.Module.IAM.Application.ModelDtos;

public class CreatePageActionCommand : CreatePageAction
{
    public string Name { get; init; } // Name of the action e.g., "Create User", "Delete User"
}

public class CreatePageAction
{
    public string Page_Nm { get; set; } // Name of the page where the action is available
    public string Action { get; set; } // The action to be performed, e.g., "Create", "Update", "Delete", "View"
    public string AccessLevel { get; set; } // Access level required to perform the action, e.g., "Read", "Write"
    public string PageUrl { get; set; } // URL of the page where the action is available
}

public class UpdatePageActionCommand : CreatePageAction
{
    [JsonIgnore]
    public Guid Id { get; set; }
}

public class DeletePageActionCommand
{
    [JsonIgnore]
    public Guid Id { get; set; }
}

public class GetPageActionQuery
{
    public Guid Id { get; init; }
}

public class GetPageActionListQuery
{
    public Guid Id { get; init; }    
    public string Name { get; init; }
    public string Page_Nm { get; init; }
    public int index { get; set; } = 1;
    public int size { get; set; } = 10;
}

