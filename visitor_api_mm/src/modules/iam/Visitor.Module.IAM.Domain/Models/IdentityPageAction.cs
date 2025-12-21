namespace Visitor.Module.IAM.Domain.Models;

public class IdentityPageAction : BaseModel
{
    public Guid Page_Id { get; set; }
    public string Name { get; set; } // Name of the action e.g., "Create User", "Delete User"
    public string Action { get; set; } // The action to be performed, e.g., "Create", "Update", "Delete", "View"
    public string AccessLevel { get; set; } // Access level required to perform the action, e.g., "Read", "Write"
    public string PageUrl { get; set; } // URL of the page where the action is available
    ////public string PageActionType { get; set; } // Type of the action, e.g., "Button", "Link", "Dropdown", "Modal"
}
