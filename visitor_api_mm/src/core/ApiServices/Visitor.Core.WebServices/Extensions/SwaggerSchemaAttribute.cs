using System.Reflection;

namespace Visitor.Core.WebServices.Extensions;

[AttributeUsage(
    AttributeTargets.Class |
    AttributeTargets.Struct |
    AttributeTargets.Parameter |
    AttributeTargets.Property |
    AttributeTargets.Enum,
    AllowMultiple = false)]
public class SwaggerSchemaAttribute : Attribute
{
    public SwaggerSchemaAttribute(string example)
    {
        Example = example;
    }

    public string Example { get; set; }
}

public class SwaggerSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.MemberInfo != null)
        {
            var schemaAttribute = context.MemberInfo.GetCustomAttributes<SwaggerSchemaAttribute>()
           .FirstOrDefault();
            if (schemaAttribute != null)
                ApplySchemaAttribute(schema, schemaAttribute);
        }
    }

    private void ApplySchemaAttribute(OpenApiSchema schema, SwaggerSchemaAttribute schemaAttribute)
    {
        if (schemaAttribute.Example != null)
        {
            schema.Example = new Microsoft.OpenApi.Any.OpenApiString(schemaAttribute.Example);
        }
    }
}