using Core.Api.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Core.Api.JwtSetup.Options
{
    internal class Jwt : IConfigureOptions<JsonWebToken>
    {
        private readonly IConfiguration _configuration;
        public Jwt(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(JsonWebToken options)
        {
            _configuration.GetSection("ContellationJWT").Bind(options);
        }
    }
}
