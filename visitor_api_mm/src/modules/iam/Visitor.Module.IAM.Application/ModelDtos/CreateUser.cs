namespace Visitor.Module.IAM.Application.ModelDtos;

public class CreateUserCommand : CreateUser
{
    public string UserName { get; set; } = null!;
}

public class CreateUser
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public bool IsEmailConfirmed { get; set; } = false;
    public string PhoneNumber { get; set; } = null!;
    public bool IsPhoneNumberConfirmed { get; set; } = false;
    public bool TwoFactorEnabled { get; set; } = false;
    public string PasswordHash { get; set; } = null!;
}

public class UpdateUserCommand : CreateUser
{
    [JsonIgnore]
    public Guid Id { get; set; }
}

public class DeleteUserCommand
{
    [JsonIgnore]
    public Guid Id { get; set; }
}

public class GetUserQuery
{
    public Guid Id { get; init; }
}

public class GetUserListQuery
{
    public Guid Id { get; init; }
    public string UserName { get; set; }
    public int index { get; set; } = 1;
    public int size { get; set; } = 10;
}

