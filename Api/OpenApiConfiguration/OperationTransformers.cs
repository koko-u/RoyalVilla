using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace RoyalVilla.Api.OpenApiConfiguration;

public static class OperationTransformers
{
    public static Task WithoutSummaryAndDescription(OpenApiOperation operation,
        OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
    {
        operation.Summary = null;
        operation.Description = null;

        return Task.CompletedTask;
    } 
}