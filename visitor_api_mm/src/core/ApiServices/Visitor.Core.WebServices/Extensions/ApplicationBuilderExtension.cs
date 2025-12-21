namespace Visitor.Core.WebServices.Extensions;

public static class ApplicationBuilderExtension
{
    public static void UseAuthenticate(this IApplicationBuilder app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }
}
