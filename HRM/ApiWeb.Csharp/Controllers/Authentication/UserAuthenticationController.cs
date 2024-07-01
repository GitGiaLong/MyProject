using Core.API.JwtSetup;
using Core.Entities.Managements.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiWeb.Csharp.Controllers.Authentication
{
    [Route("Authentication")]
    [ApiController]
    [Authorize]
    public class UserAuthenticationController : ControllerBase
    {
        private readonly IBuildJwt _jwtProvider;
        public UserAuthenticationController(IBuildJwt jwtHelper)
        {
            _jwtProvider = jwtHelper;
        }

        [AllowAnonymous]//test 
        [HttpPost]
        public IActionResult Login([FromBody] User loginModel)
        {
            // Thay thế phần này bằng logic xác thực người dùng của bạn
            if (loginModel.Username == "AdminSSR" && loginModel.Password == "123456")
            {
                string token = _jwtProvider.GenerateToken(loginModel);

                return Ok(new
                {
                    token
                    //= new JwtSecurityTokenHandler().WriteToken(token),
                    //expiration = token.ValidTo
                });
            }

            return Unauthorized();
        }
    }
}
