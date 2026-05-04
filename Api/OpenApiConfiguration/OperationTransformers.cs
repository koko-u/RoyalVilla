using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace RoyalVilla.Api.OpenApiConfiguration;

/// <summary>
/// OpenApi Operation Transformations
/// </summary>
public static class OperationTransformers
{
    /// <summary>
    /// Exclude Summary and Description XML Document comments from OpenApi operations
    /// </summary>
    /// <param name="operation"></param>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static Task WithoutSummaryAndDescription(OpenApiOperation operation,
        OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
    {
        operation.Summary = null;
        operation.Description = null;

        return Task.CompletedTask;
    } 
}