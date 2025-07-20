using Microsoft.AspNetCore.ResponseCompression;

namespace Blazor.Contellation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //builder.Services.ConfigureCors();
            // Add services to the container.

            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            //builder.Services.ConfigureJWT();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseCors("ContellationOrigin");
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();
            //app.UseForwardedHeaders(new ForwardedHeadersOptions
            //{
            //    // will forward proxy headers to the current request. This will help us during the Linux deployment.
            //    ForwardedHeaders = ForwardedHeaders.All
            //});
            app.UseRouting();


            app.UseAuthorization();
            app.MapRazorPages();
            app.MapControllers();
            app.MapFallbackToFile("index.html");

            app.Run();
        }
    }
}
