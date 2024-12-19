using Core.Api.Services;
using Microsoft.AspNetCore.HttpOverrides;

namespace WebApi.Cshrap
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.ConfigureDbContext();

            builder.Services.ConfigureCors();
            builder.Services.ConfigureRepositoryWrapper();
            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.ConfigureLoggerService();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Authenticatte JWT
            builder.Services.ConfigureJWT();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => {
                        c.InjectStylesheet("/css/swagger-ui/ui.css");// css path
                        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiWeb.Csharp v1");
                        c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
                    }
                );
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors("ContellationOrigin");

            app.UseStaticFiles();
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                // will forward proxy headers to the current request. This will help us during the Linux deployment.
                ForwardedHeaders = ForwardedHeaders.All
            });
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
