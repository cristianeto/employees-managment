using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace ProCredit.EmployeeManagement.Api.OpenApi;

public class BearerSecuritySchemeTransformer : IOpenApiDocumentTransformer
{
    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        var scheme = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
        };

        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
        document.Components.SecuritySchemes["Bearer"] = scheme;

        var requirement = new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference("Bearer", document)] = [],
        };

        if (document.Paths is not null)
        {
            foreach (var path in document.Paths.Values)
            {
                if (path is null)
                {
                    continue;
                }

                if (path.Operations is null)
                {
                    continue;
                }

                foreach (var operation in path.Operations.Values)
                {
                    operation.Security ??= [];
                    operation.Security.Add(requirement);
                }
            }
        }

        return Task.CompletedTask;
    }
}
