using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Core.Api.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;

namespace Core.Api.JwtSetup.Options
{
    internal class JwtBearer : IConfigureNamedOptions<JwtBearerOptions>
    {
        private readonly JsonWebToken _jwtOptions;
        public JwtBearer(IOptions<JsonWebToken> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }

        public void Configure(string name, JwtBearerOptions options)
        {
            if (name == "Bearer")
            {
                Configure(options);
            }
        }

        public void Configure(JwtBearerOptions options)
        {
            options.RequireHttpsMetadata = true;
            options.SaveToken = true;
            options.TokenValidationParameters = new()
            {
                //Same Secret key will be used while creating the token
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtOptions.SecretKey)),
                ValidateIssuerSigningKey = true,

                //Usually, this is your application base URL
                ValidIssuer = _jwtOptions.Issuer,
                ValidateIssuer = true,

                //Here, we are creating and using JWT within the same application.
                //In this case, base URL is fine.
                //If the JWT is created using a web service, then this would be the consumer URL.
                ValidAudience = _jwtOptions.Audience,
                ValidateAudience = true,

                RequireExpirationTime = true,
                ValidateLifetime = true,

                ClockSkew = TimeSpan.Zero
            };

            options.Events = new()
            {
                OnTokenValidated = context =>
                {
                    if (context.SecurityToken is JwtSecurityToken accessToken)
                    {
                        ClaimsPrincipal claimPrincipal = context.Principal;
                        GetToken.Token = accessToken;
                        GetToken.Code = claimPrincipal.FindFirst(ClaimTypes.Name).Value;
                    }

                    return Task.CompletedTask;
                },
                OnAuthenticationFailed = context =>
                {
                    if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                    {
                        context.Response.Headers.Add("Token-Expired", "true");
                        context.Response.ContentType = "application/json";
                    }
                    GetToken.Reset();
                    return Task.CompletedTask;
                },
                OnMessageReceived = ctx =>
                {

                    ctx.Request.Headers.TryGetValue("Authorization", out Microsoft.Extensions.Primitives.StringValues BearerToken);
                    if (BearerToken.Count == 0)
                    {
                        BearerToken = "no Bearer token sent\n";
                    }

                    return Task.CompletedTask;
                },
                OnChallenge = context =>
                {
                    string authorization = context.Request.Headers[HeaderNames.Authorization];

                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "text/plain";
                    return context.Response.WriteAsync("Unauthorized");

                }
            };
        }
    }
}
