using Core.Library.Authentications;
using Core.Library.Services.HttpServices;
using Core.Library.Services.LocalStorages;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Library.Services
{
    public static class Extensions
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
            services.AddScoped<IAuthentication, Authentication>();
        }
    }
}
