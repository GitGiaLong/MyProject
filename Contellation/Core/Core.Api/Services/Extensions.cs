using Core.Api.Infrastractures.Repositories;
using Core.Api.JwtSetup;
using Core.Api.JwtSetup.Options;
using Core.Libraries.Connects;
using Core.Libraries.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Core.Api.Services
{
    public static class Extensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("ContellationOrigin",
                builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });
        }

        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            //services.AddSingleton<ILoggerManager, LoggerManager>();
        }

        //private static ConnectServer _ConnectServer = new ConnectServer();
        //public static ConnectServer Connect { get { return _ConnectServer; } set { _ConnectServer = value; } }

        public static void ConfigureDbContext(this IConfiguration configuration)
        {

            //Connect = configuration.GetSection("ConnectServer").Get<List<ConnectServer>>()[0];

        }

        public static void ConfigureRepositoryWrapper(this IServiceCollection services)
        {
            services.AddScoped<IConnectServers, ConnectServers>();
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
            services.AddScoped<IBuildJwt, BuildJwt>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        }

        public static void ConfigureJWT(this IServiceCollection services)
        {
            services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();

            // Đọc thông tin JWT
            services.ConfigureOptions<Jwt>();
            // setup các thông tin JwtBearerOptions cho JWT
            services.ConfigureOptions<JwtBearer>();
        }

    }
}
