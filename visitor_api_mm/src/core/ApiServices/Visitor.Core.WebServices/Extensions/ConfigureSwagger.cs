using Microsoft.AspNetCore.Routing;

namespace Visitor.Core.WebServices.Extensions;

public static class ConfigureSwagger
{
    public static void AddCustomSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<RouteOptions>(o =>
        {
            o.LowercaseUrls = true;
            o.LowercaseQueryStrings = true;
        });

        builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(options =>
        {
            options.EnableAnnotations();
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "bearer",
                BearerFormat = "JWT"
            });
            options.OperationFilter<SecurityRequirementsOperationFilter>();
            options.AddSecurityRequirement(new OpenApiSecurityRequirement{
            {
                new OpenApiSecurityScheme{Reference = new OpenApiReference{Type = ReferenceType.SecurityScheme, Id = "Bearer"}},
                Array.Empty<string>()
            }
            });
        });

        builder.Services.Configure<SwaggerUIOptions>(options =>
        {
            options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
        });
    }


    public static void UseCustomSwagger(this IApplicationBuilder app, IApiVersionDescriptionProvider provider)
    {
        app.UseSwagger();

        app.UseSwaggerUI(options =>
        {
            foreach (var desc in provider.ApiVersionDescriptions)
            {
                var docName = $"{desc.GroupName}".ToLowerInvariant();
                options.SwaggerEndpoint($"/swagger/{docName}/swagger.json",
                    $"{desc.GroupName.Split('-')[0].ToUpperInvariant()}");
            }

            options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
        });
    }    
}
public class SecurityRequirementsOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (!context.MethodInfo.GetCustomAttributes(true).Any(x => x is AllowAnonymousAttribute))
        {
            operation.Security = new List<OpenApiSecurityRequirement>()
            {
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                }
            };
        }
        else
        {
            operation.Responses.Remove("401");
        }
    }
}
