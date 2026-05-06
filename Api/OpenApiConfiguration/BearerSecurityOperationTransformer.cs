using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace RoyalVilla.Api.OpenApiConfiguration;

/// <summary>
/// Operation transformer to add Bearer security scheme to OpenAPI operations
/// </summary>
public sealed class BearerSecurityOperationTransformer : IOpenApiOperationTransformer
{
    /// <inheritdoc />
    public Task TransformAsync(
        OpenApiOperation operation, 
        OpenApiOperationTransformerContext context,
        CancellationToken cancellationToken)
    {
        var metadata = context.Description.ActionDescriptor.EndpointMetadata;

        if (metadata.OfType<AllowAnonymousAttribute>().Any())
        {
            // Allow Anonymous action method
            return Task.CompletedTask;
        }

        if (!metadata.OfType<AuthorizeAttribute>().Any())
        {
            // No Authorize attributes on action method
            return Task.CompletedTask;
        }

        var bearerSecurityKey = new OpenApiSecuritySchemeReference(JwtBearerDefaults.AuthenticationScheme, context.Document);

        operation.Security ??= new List<OpenApiSecurityRequirement>();
        operation.Security.Add(new OpenApiSecurityRequirement
        {
            [bearerSecurityKey] = []
        });
        
        return Task.CompletedTask;
    }
}