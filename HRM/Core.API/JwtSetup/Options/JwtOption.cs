using Core.Entities.Applications.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Core.API.JwtSetup.Options
{
    public class JwtOption : IConfigureOptions<JsonWebToken>
    {
        private readonly IConfiguration _configuration;
        public JwtOption(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(JsonWebToken options)
        {
            _configuration.GetSection("ContellationJWT").Bind(options);
        }
    }
}
