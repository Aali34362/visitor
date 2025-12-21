namespace Visitor.Module.IAM.Application.Mappers;

public class CreateIAMProfilers : Profile
{
    public CreateIAMProfilers()
    {
        //Module
        CreateMap<GetModuleListQuery, IdentityModule>();
        CreateMap<CreateModuleCommand, IdentityModule>()
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => Serialize(src.Tags)));
        CreateMap<UpdateModuleCommand, IdentityModule>()
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => Serialize(src.Tags)));

        //Page
        CreateMap<CreatePageCommand, IdentityPage>();
        CreateMap<UpdatePageCommand, IdentityPage>();
        CreateMap<GetPageListQuery, IdentityPage>();

        CreateMap<CreatePageActionCommand, IdentityPageAction>();
        CreateMap<UpdatePageActionCommand, IdentityPageAction>();
        CreateMap<GetPageActionListQuery, IdentityPageAction>();

        CreateMap<CreatePolicyCommand, IdentityPolicy>()
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => Serialize(src.Tags)));
        CreateMap<UpdatePolicyCommand, IdentityPolicy>()
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => Serialize(src.Tags)));
        CreateMap<GetPolicyListQuery, IdentityPolicy>();

        CreateMap<CreateRoleCommand, IdentityRole>()
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => Serialize(src.Tags)));
        CreateMap<UpdateRoleCommand, IdentityRole>()
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => Serialize(src.Tags)));
        CreateMap<GetRoleListQuery, IdentityRole>();

        CreateMap<CreateUserCommand, IdentityUser>();
        CreateMap<UpdateUserCommand, IdentityUser>();
        CreateMap<GetUserListQuery, IdentityUser>();

        CreateMap<CreateUserLoginCommand, IdentityUserLogin>();
        CreateMap<UpdateUserLoginCommand, IdentityUserLogin>();
        CreateMap<GetUserLoginListQuery, IdentityUserLogin>();
    }
    public static string Serialize<T>(T value)
    {
        return System.Text.Json.JsonSerializer.Serialize(value, new System.Text.Json.JsonSerializerOptions());
    }
    public static T Deserialize<T>(string json)
    {
        return System.Text.Json.JsonSerializer.Deserialize<T>(json, new System.Text.Json.JsonSerializerOptions());
    }
}
