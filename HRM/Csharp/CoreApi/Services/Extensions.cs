using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csharp.CoreApi.Services
{
    public static class Extensions
    {
        //public static void ConfigureCors(this IServiceCollection services)
        //{
        //    services.AddCors(options =>
        //    {
        //        options.AddPolicy("ContellationOrigin",
        //        builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
        //    });
        //}

        //public static void ConfigureLoggerService(this IServiceCollection services)
        //{
        //    //services.AddSingleton<ILoggerManager, LoggerManager>();
        //}

        //private static ConnectServer _ConnectServer = new ConnectServer();
        //public static ConnectServer Connect { get { return _ConnectServer; } set { _ConnectServer = value; } }

        //public static void ConfigureDbContext(this IConfiguration configuration)
        //{

        //    Connect = configuration.GetSection("ConnectServer").Get<List<ConnectServer>>()[0];

        //}

        //public static void ConfigureRepositoryWrapper(this IServiceCollection services)
        //{
        //    //services.AddScoped<IMongoContext, MongoContext>();
        //    //services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
        //    services.AddScoped<IBuildJwt, BuildJwt>();
        //    services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        //}

        //public static void ConfigureJWT(this IServiceCollection services)
        //{
        //    services
        //    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        //    .AddJwtBearer();

        //    // Đọc thông tin JWT
        //    services.ConfigureOptions<JwtOption>();
        //    // setup các thông tin JwtBearerOptions cho JWT
        //    services.ConfigureOptions<JwtBearerOption>();
        //}

    }
}
