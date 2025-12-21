namespace Visitor.Module.IAM.Application.ModelDtos;

public class CreateUserRoleMappingCommand : CreateUserRoleMapping
{
}

public class CreateUserRoleMapping
{
    public string User_Nm { get; set; }
    public string Role_Nm { get; set; }
}

public class UpdateUserRoleMappingCommand : CreateUserRoleMapping
{
    [JsonIgnore]
    public Guid Id { get; set; }
}

public class DeleteUserRoleMappingCommand
{
    [JsonIgnore]
    public Guid Id { get; set; }
}

public class GetUserRoleMappingQuery
{
    public Guid Id { get; init; }
}

public class GetUserRoleMappingListQuery
{
    public Guid Id { get; init; }
    public string User_Nm { get; set; }
    public string Role_Nm { get; set; }
    public int index { get; set; } = 1;
    public int size { get; set; } = 10;
}

