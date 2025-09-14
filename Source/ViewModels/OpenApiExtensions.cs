using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace MinimalApi.ViewModels
{
    public static class OpenApiExtensions
    {

    }

    internal sealed class BearerSecuritySchemeTransformer(IAuthenticationSchemeProvider authenticationSchemeProvider) : IOpenApiDocumentTransformer
    {
        public async Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
        {
            var authenticationSchemes = await authenticationSchemeProvider.GetAllSchemesAsync();
            if (authenticationSchemes.Any(authScheme => authScheme.Name == "Bearer"))
            {
                var requirements = new Dictionary<string, IOpenApiSecurityScheme>
                {
                    ["Bearer"] = new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer", // "bearer" refers to the header name here
                        In = ParameterLocation.Header,
                        BearerFormat = "Json Web Token"
                    }
                };
                document.Components ??= new OpenApiComponents();
                document.Components.SecuritySchemes = requirements;
            }
        }
    }

    internal sealed class AddVersionToHeaderTransformer : IOpenApiOperationTransformer
    {
        public Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
        {
            var actionMetadata = context.Description.ActionDescriptor.EndpointMetadata;
            if (actionMetadata != null)
            {
                operation.Parameters ??= [];
                var apiVersionMetadata = actionMetadata.Any(metadataItem => metadataItem is ApiVersionMetadata);
                if (apiVersionMetadata)
                {
                    operation.Parameters.Add(new OpenApiParameter
                    {
                        Name = "API-Version",
                        In = ParameterLocation.Header,
                        Description = "API Version header value",
                        Schema = new OpenApiSchema
                        {
                            Type = JsonSchemaType.String,
                            Description = "API Version"
                        }
                    });
                }
            }

            return Task.CompletedTask;
        }
    }
}
