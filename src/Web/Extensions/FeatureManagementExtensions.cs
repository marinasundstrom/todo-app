using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.FeatureFilters;

namespace TodoApp.Web.Extensions;

public static class FeatureManagementExtensions
{
    public static IServiceCollection AddFeatureManagement(this IServiceCollection services)
    {
        Microsoft.FeatureManagement.ServiceCollectionExtensions.AddFeatureManagement(services)
                .AddFeatureFilter<PercentageFilter>()
                .AddFeatureFilter<TimeWindowFilter>();

        return services;
    }
}
