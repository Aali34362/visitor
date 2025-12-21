namespace Visitor.Module.IAM.Application.ModelDtos;

public class CreateUserLoginCommand : CreateUserLogin
{
    public string UserName { get; set; }
    public Guid Session_Id { get; set; }
    public string Login_Source_Sytem { get; set; }
    public string Login_Source_Sytem_Ip { get; set; }
    public DateTime Login_Date { get; set; }
}

public class CreateUserLogin
{
    public DateTime Logout_Date { get; set; }
}

public class UpdateUserLoginCommand : CreateUserLogin
{
    [JsonIgnore]
    public Guid Id { get; set; }
}


public class GetUserLoginListQuery
{
    public Guid Id { get; init; }
    public string UserName { get; set; }
    public int index { get; set; } = 1;
    public int size { get; set; } = 10;
}

