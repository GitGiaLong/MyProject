using Core.Libraries.Authentication;
using Core.Libraries.Services.HttpServices;
using Core.Libraries.Services.LocalStorages;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Libraries.Services
{
    public static class ServiceExtensions
    {
        ///Client

        /// <summary>
        /// Configure Repository Client
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureRepositoryClient(this IServiceCollection services)
        {
            services.AddScoped<IHttpService, HttpService>();
            services.AddScoped<ILocalStorage, LocalStorage>();
            services.AddScoped<IAuthentication, Authentication.Authentication>();
        }
    }
}
