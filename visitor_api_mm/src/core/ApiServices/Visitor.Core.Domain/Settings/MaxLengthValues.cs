namespace Visitor.Core.Domain.Settings;

public readonly struct MaxLengthValues
{
    //Global
    public const int Tags = 1000;

    //One Identity
    //User
    public const int UserName = 50;
    public const int FirstName = 50;
    public const int LastName = 50;
    public const int Email = 50;
    public const int PhoneNumber = 50;
    public const int PasswordHash = 50;
    //Role
    public const int RoleName = 50;
    //User Login
    public const int LoginSourceSystem = 50;
    public const int LoginSourceSystemIp = 50;
    //Module
    public const int ModuleName = 150;
    //Page
    public const int PageTitle = 150;
    public const int PageUrl = 150;
    public const int PageName = 150;
    public const int Icon = 50;
    //Page Action
    public const int PageActionName = 150;
    public const int PageActionUrl = 150;
    public const int PageActionAccessLevel = 50;
    public const int PageActionAction = 50;
    //Policy
    public const int PolicyName = 150;

    //DMS
    //Document Category
    public const int DocumentCategoryName = 150;
    //Document Type
    public const int DocumentTypeName = 150;
}
