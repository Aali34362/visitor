namespace Visitor.Module.IAM.Application.ModelDtos;

public class CreateRolePolicyMappingCommand : CreateRolePolicyMapping
{
}

public class CreateRolePolicyMapping
{
    public string Policy_Nm { get; set; }
    public string Role_Nm { get; set; }
}

public class UpdateRolePolicyMappingCommand : CreateRolePolicyMapping
{
    [JsonIgnore]
    public Guid Id { get; set; }
}

public class DeleteRolePolicyMappingCommand
{
    [JsonIgnore]
    public Guid Id { get; set; }
}

public class GetRolePolicyMappingQuery
{
    public Guid Id { get; init; }
}

public class GetRolePolicyMappingListQuery
{
    public Guid Id { get; init; }
    public string Policy_Nm { get; set; }
    public string Role_Nm { get; set; }
    public int index { get; set; } = 1;
    public int size { get; set; } = 10;
}

