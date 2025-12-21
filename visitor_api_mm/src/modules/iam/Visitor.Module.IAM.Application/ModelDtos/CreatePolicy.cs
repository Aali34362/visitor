namespace Visitor.Module.IAM.Application.ModelDtos;

public class CreatePolicyCommand : CreatePolicy
{
    public string Name { get; init; }
}

public class CreatePolicy
{
    public Dictionary<string,string> Tags { get; set; }
}

public class UpdatePolicyCommand : CreatePolicy
{
    [JsonIgnore]
    public Guid Id { get; set; }
}

public class DeletePolicyCommand
{
    [JsonIgnore]
    public Guid Id { get; set; }
}

public class GetPolicyQuery
{
    public Guid Id { get; init; }
}

public class GetPolicyListQuery
{   
    public string Name { get; init; }
    public int index { get; set; } = 1;
    public int size { get; set; } = 10;
}

