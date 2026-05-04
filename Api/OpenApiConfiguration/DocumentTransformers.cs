using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace RoyalVilla.Api.OpenApiConfiguration;

public static class DocumentTransformers
{
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