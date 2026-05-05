using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Common.Annotations;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace Common.OpenApiConfiguration;

public static class OpenApiRequiredDocument
{
    /// <summary>
    /// OpenApiRequired property make required schema transformer
    /// </summary>
    /// <param name="asm"></param>
    /// <returns></returns>
    public static Func<OpenApiDocument, OpenApiDocumentTransformerContext, CancellationToken, Task>
        OpenApiRequiredTransformer(Assembly asm)
    {
        return (document, context, cancellationToken) =>
        {
            // concrete classes which has OpenApiRequired property 
            var dtoTypes = asm.GetTypes().Where(IsConcrete).Where(HasRequiredProperty);

            foreach (var dtoType in dtoTypes)
            {
                var schemaName = dtoType.Name;

                if (document.Components?.Schemas is null)
                {
                    continue;
                }

                if (!document.Components.Schemas.TryGetValue(schemaName, out var schema))
                {
                    continue;
                }

                // found dtoType's schema

                // dtoType's required property names
                var requiredProperties =
                    dtoType.GetProperties().Where(HasRequiredAttribute).Select(GetJsonPropertyName);

                foreach (var requiredPropertyName in requiredProperties)
                {
                    if (schema.Properties?.ContainsKey(requiredPropertyName) is false)
                    {
                        continue;
                    }

                    schema.Required?.Add(requiredPropertyName);
                }
            }

            return Task.CompletedTask;
        };
    }

    static bool IsConcrete(Type t) => t is { IsClass: true, IsAbstract: false };

    static bool HasRequiredProperty(Type t) => t.GetProperties().Any(HasRequiredAttribute);

    static bool HasRequiredAttribute(PropertyInfo p) => p.GetCustomAttribute<OpenApiRequiredAttribute>() is not null;

    static string GetJsonPropertyName(PropertyInfo p)
    {
        var jsonPropertyName = p.GetCustomAttribute<JsonPropertyNameAttribute>();
        if (jsonPropertyName is not null)
        {
            return jsonPropertyName.Name;
        }

        // ASP.NET Core Web API Default Naming convention is camelCase
        return JsonNamingPolicy.CamelCase.ConvertName(p.Name);
    }
}