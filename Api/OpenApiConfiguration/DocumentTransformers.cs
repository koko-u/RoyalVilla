using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace RoyalVilla.Api.OpenApiConfiguration;

/// <summary>
/// OpenApi Document Transformers
/// </summary>
public static class DocumentTransformers
{
    /// <summary>
    /// Configure OpenApi document tags with descriptions
    /// </summary>
    /// <param name="doc"></param>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static Task TagDescriptions(OpenApiDocument doc, OpenApiDocumentTransformerContext context,
        CancellationToken cancellationToken)
    {
        doc.Tags = new HashSet<OpenApiTag>
        {
            new() { Name = "Villas", Description = "Manage villa properties and reservations" }
        };

        return Task.CompletedTask;
    }
}