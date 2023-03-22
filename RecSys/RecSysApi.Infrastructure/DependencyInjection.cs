using Microsoft.Extensions.DependencyInjection;
using RecSysApi.Domain.Interfaces.Repositories;
using RecSysApi.Domain.Interfaces.Services;
using RecSysApi.Domain.Interfaces.UnitOfWork;
using RecSysApi.Infrastructure.Implementations.Repositories;
using RecSysApi.Infrastructure.Implementations.Services;
using RecSysApi.Infrastructure.Implementations.UnitOfWork;

namespace RecSysApi.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureLayerDependencies(this IServiceCollection services)
    {
        services.AddSingleton<IHttpService, HttpService>();
        services.AddSingleton<IDigestAuthService, DigestAuthService>();

        services.AddScoped<IFeedbackRepository, FeedbackRepository>();
        services.AddScoped<IQueryRepository, QueryRepository>();
        services.AddScoped<IUpdateRepository, UpdateRepository>();
        services.AddScoped<IVideoRepository, VideoRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }
}