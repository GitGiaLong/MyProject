using Microsoft.AspNetCore.HttpOverrides;
using WebAPICsharp.Service.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureCors();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCors("CorsPolicy");

app.UseStaticFiles();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    // will forward proxy headers to the current request. This will help us during the Linux deployment.
    ForwardedHeaders = ForwardedHeaders.All
});
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(
            c =>
            {
                //c.InjectStylesheet("/css/swagger-ui/ui.css");// css path
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "HRM_API v1");
                c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
            }
        );
}

app.UseHttpsRedirection();


app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
