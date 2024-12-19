using Core.Api.Infrastractures.Repositories;
using Core.Api.Infrastractures.Repositories.UserManagement;
using Core.Api.JwtSetup;
using Core.Entities.UserManagement;
using Core.Libraries.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace WebApi.Cshrap.Controllers.Authentication
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserAuthenticationController : ControllerBase
    {
        private readonly IBuildJwt _jwtProvider;
        private readonly IRepositoryWrapper _repository;

        public UserAuthenticationController(IBuildJwt jwtHelper, IRepositoryWrapper repository)
        {
            _jwtProvider = jwtHelper;
            _repository = repository;
        }

        [AllowAnonymous]//test 
        [HttpPost]
        public IActionResult Login([FromBody] Account loginModel)
        {
            XML xML = new XML();
            //var a = xML.ReadXML("Connect", "ServerName");
            //xML.SerializeSettingToFile(xML.FILE_PATH);
            // Thay thế phần này bằng logic xác thực người dùng của bạn
            //if (loginModel.Username == "AdminSSR" && loginModel.Password == "123456")
            if (!loginModel.Username.IsNullOrEmpty() && !loginModel.Password.IsNullOrEmpty())
            {

                _repository.UserIdentity.Identity(loginModel);
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
