namespace Visitor.Module.IAM.Application.ModelDtos;

public class CreatePolicyPageActionMappingCommand : CreatePolicyPageActionMapping
{
}

public class CreatePolicyPageActionMapping
{
    public string Policy_Nm { get; set; }
    public string PageAction_Nm { get; set; }
}

public class UpdatePolicyPageActionMappingCommand : CreatePolicyPageActionMapping
{
    [JsonIgnore]
    public Guid Id { get; set; }
}

public class DeletePolicyPageActionMappingCommand
{
    [JsonIgnore]
    public Guid Id { get; set; }
}

public class GetPolicyPageActionMappingQuery
{
    public Guid Id { get; init; }
}

public class GetPolicyPageActionMappingListQuery
{
    public Guid Id { get; init; }
    public string Policy_Nm { get; set; }
    public string PageAction_Nm { get; set; }
    public int index { get; set; } = 1;
    public int size { get; set; } = 10;
}

