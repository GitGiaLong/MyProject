using Core.API.Services;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.ConfigureDbContext();

builder.Services.ConfigureCors();
builder.Services.ConfigureRepositoryWrapper();

builder.Services.AddControllers();
builder.Services.ConfigureLoggerService();
// Authenticatte JWT
builder.Services.ConfigureJWT();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

#region Swagger Configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Contellation", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. 
                      Enter your token in the text input below.
                      Example: 'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiQWRtaW5TU1IiLCJuYmYiOjE3MTc4Njg5ODgsImV4cCI6MTcxODAzODgwMCwiaXNzIjoiQ29udGVsbGF0aW9uX0FkbWluU1NSIiwiYXVkIjoiTWFuYWdlbWVudF9IUk0ifQ.JXw1fgfCUY1mcH8A2Vpmc-UgeCrzjSIYmX6YGF_2ER0'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                }
            },
            new List<string>()
        }
    });
});
#endregion
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    app.UseSwagger();
    app.UseSwaggerUI(
        c =>
        {
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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
