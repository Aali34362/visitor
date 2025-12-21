namespace Visitor.Module.IAM.Application.ModelDtos;

public class CreateModuleCommand : CreateModule
{
    public string Name { get; init; } = null!;   
}

public class CreateModule
{
    public Dictionary<string, string> Tags { get; init; }
}

public class UpdateModuleCommand : CreateModule
{
    [JsonIgnore]
    public Guid Id { get; set; }
}

public class DeleteModuleCommand
{
    [JsonIgnore]
    public Guid Id { get; set; }
}

public class GetModuleQuery
{
    public Guid Id { get; init; }
}

public class GetModuleListQuery
{
    public string Name { get; init; }
    public int index { get; init; } = 1;
    public int size { get; init; } = 10;
}