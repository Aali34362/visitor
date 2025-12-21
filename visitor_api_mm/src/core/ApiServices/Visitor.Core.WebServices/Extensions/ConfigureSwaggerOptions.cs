using System.Text.RegularExpressions;

namespace Visitor.Core.WebServices.Extensions;

public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;
    private readonly IConfiguration _configuration;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider, IConfiguration configuration)
    {
        _provider = provider;
        _configuration = configuration;
    }

    public void Configure(SwaggerGenOptions options)
    {
        var swaggerConfig = _configuration.GetSection("OpenApi");
        foreach (var desc in _provider.ApiVersionDescriptions)
        {
            var docName = $"{desc.GroupName}".ToLowerInvariant();
            options.SwaggerDoc(docName, new OpenApiInfo
            {
                Title = $"{swaggerConfig["Title"]} - {desc.GroupName.Split('-')[0].ToUpperInvariant()}",
                Version = desc.GroupName,
                Description = swaggerConfig["Description"] ?? "Default API Description",
                Contact = new OpenApiContact
                {
                    Name = swaggerConfig["ContactName"] ?? "Default Contact Name",
                    Email = swaggerConfig["ContactEmail"] ?? "Default Contact Email"
                }
            });
        }

        options.DocInclusionPredicate((docName, apiDesc) =>
            string.Equals(apiDesc.GroupName, docName, StringComparison.OrdinalIgnoreCase));
    }
}
