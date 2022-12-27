using Microsoft.AspNetCore.Mvc.ApiExplorer;
using NSwag;
using NSwag.Generation.Processors.Security;

namespace TodoApp.Web.Extensions;

public static class OpenApiExtensions
{
    public static IServiceCollection AddOpenApi(this IServiceCollection services, WebApplicationBuilder builder)
    {
#pragma warning disable ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'
        var provider = builder.Services
            .BuildServiceProvider()
            .GetRequiredService<IApiVersionDescriptionProvider>();
#pragma warning restore ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'

        foreach (ApiVersionDescription description in provider.ApiVersionDescriptions)
        {
            services.AddOpenApiDocument(config =>
            {
                config.DocumentName = $"v{description.ApiVersion}";
                config.PostProcess = document =>
                {
                    document.Info.Title = "Todo API";
                    document.Info.Version = $"v{description.ApiVersion.ToString()}";
                };
                config.ApiGroupNames = new[] { description.ApiVersion.ToString() };

                config.AddSecurity("JWT", new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    In = OpenApiSecurityApiKeyLocation.Header,
                    Description = "Type into the textbox: Bearer {your JWT token}."
                });

                config.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));

                config.SchemaNameGenerator = new CustomSchemaNameGenerator();
            });
        }

        return services;
    }
}
