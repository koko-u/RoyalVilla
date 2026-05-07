using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.OpenApiTags;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace RoyalVilla.Api.OpenApiConfiguration;

/// <summary>
/// Configure OpenApi document tags with descriptions
/// </summary>
public sealed class TagsDescriptionDocumentTransformer : IOpenApiDocumentTransformer
{
    /// <inheritdoc />
    public Task TransformAsync(
        OpenApiDocument document,
        OpenApiDocumentTransformerContext context,
        CancellationToken cancellationToken
    )
    {
        document.Tags = ApiTags
            .List.Select(tag => new OpenApiTag { Name = tag.Name, Description = tag.Description() })
            .ToHashSet();

        return Task.CompletedTask;
    }
}
