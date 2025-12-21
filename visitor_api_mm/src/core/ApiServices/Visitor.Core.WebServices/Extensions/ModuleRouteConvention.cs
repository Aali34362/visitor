using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Text.RegularExpressions;

namespace Visitor.Core.WebServices.Extensions;

public sealed class ModuleRouteConvention : IApplicationModelConvention
{
    private static readonly Regex Rx =
        new(@"\.Module\.(?<module>[A-Za-z0-9_]+)\.Controllers\.", RegexOptions.Compiled);

    public void Apply(ApplicationModel application)
    {
        foreach (var controller in application.Controllers)
        {
            var ns = controller.ControllerType.Namespace ?? string.Empty;
            var m = Rx.Match(ns);
            if (!m.Success) continue;

            var module = m.Groups["module"].Value.ToLowerInvariant();

            foreach (var selector in controller.Selectors)
            {
                var route = selector.AttributeRouteModel;
                if (route is null) continue;
                route.Template = route.Template!.Replace("[module]", module);
            }

            var versionModel = controller.Attributes.OfType<ApiVersionAttribute>().FirstOrDefault();
            var apiVersion = versionModel?.Versions.FirstOrDefault();
            var version = apiVersion == null ? "v1" : $"v{apiVersion.MajorVersion}";

            controller.ApiExplorer.GroupName = $"{module}-{version}";
        }
    }
}