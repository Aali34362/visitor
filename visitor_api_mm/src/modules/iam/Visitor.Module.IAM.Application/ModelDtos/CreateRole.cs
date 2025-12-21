namespace Visitor.Module.IAM.Application.ModelDtos;

public class CreateRoleCommand : CreateRole
{
    public string Name { get; set; } = null!;
}

public class CreateRole
{
    public Dictionary<string,string> Tags { get; set; }
}

public class UpdateRoleCommand : CreateRole
{
    [JsonIgnore]
    public Guid Id { get; set; }
}

public class DeleteRoleCommand
{
    [JsonIgnore]
    public Guid Id { get; set; }
}

public class GetRoleQuery
{
    public Guid Id { get; init; }
}

public class GetRoleListQuery
{
    public Guid Id { get; init; }
    public string Name { get; set; }
    public int index { get; set; } = 1;
    public int size { get; set; } = 10;
}

