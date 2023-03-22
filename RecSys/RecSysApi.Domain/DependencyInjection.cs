using Microsoft.Extensions.DependencyInjection;

namespace RecSysApi.Domain
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDomainLayerDependencies(this IServiceCollection services)
        {
            return services;
        }
    }
}