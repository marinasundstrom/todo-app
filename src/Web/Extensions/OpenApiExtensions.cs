using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
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
                config.DocumentName = $"v{GetApiVersion(description)}";
                config.PostProcess = document =>
                {
                    document.Info.Title = "Todo API";
                    document.Info.Version = $"v{GetApiVersion(description)}";
                };
                config.ApiGroupNames = new[] { GetApiVersion(description) };

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

    private static string GetApiVersion(ApiVersionDescription description)
    {
        var apiVersion = description.ApiVersion;
        return (apiVersion.MinorVersion == 0
            ? apiVersion.MajorVersion.ToString()
            : apiVersion.ToString())!;
    }
}
