﻿namespace WebAPICsharp.Service.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options => {
                options.AddPolicy("CorsPolicy", buider =>
                buider.AllowAnyOrigin()
                .AllowAnyHeader().AllowAnyMethod());
            });
        }
    }
}
