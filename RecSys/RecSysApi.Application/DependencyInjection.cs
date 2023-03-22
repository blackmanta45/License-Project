using Microsoft.Extensions.DependencyInjection;
using RecSysApi.Application.Interfaces;
using RecSysApi.Application.Interfaces.Update;
using RecSysApi.Application.Interfaces.VideosLookup;
using RecSysApi.Application.Servants;
using RecSysApi.Application.Services;
using RecSysApi.Domain.Interfaces.Services;

namespace RecSysApi.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationLayerDependencies(this IServiceCollection services)
        {
            services.AddScoped<IFeedbackService, FeedbackService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IQueriesService, QueriesService>();
            services.AddScoped<ISearchEngineService, SearchEngineService>();
            services.AddScoped<IUpdateService, UpdateService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IVideosLookupService, VideosLookupService>();

            services.AddScoped<IVideosLookupServant, VideosLookupServant>();
            services.AddScoped<IUpdateServant, UpdateServant>();

            return services;
        }
    }
}