using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace RoyalVilla.Api.OpenApiConfiguration;

/// <summary>
/// Exclude Summary and Description XML Document comments from OpenApi operations
/// </summary>
public sealed class WithoutSummaryAndDescriptionOperationTransformer : IOpenApiOperationTransformer
{
    /// <inheritdoc />
    public Task TransformAsync(
        OpenApiOperation operation,
        OpenApiOperationTransformerContext context,
        CancellationToken cancellationToken
    )
    {
        operation.Summary = null;
        operation.Description = null;

        return Task.CompletedTask;
    }
}
