using Core.API.Infrastracture.Repositories.UserManagement;
using Core.API.JwtSetup;
using Core.Entities.Managements.User;
using Core.Library.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Xml.Linq;

namespace ApiWeb.Csharp.Controllers.Authentication
{
    [Route("Authentication")]
    [ApiController]
    [Authorize]
    public class UserAuthenticationController : ControllerBase
    {
        private readonly IBuildJwt _jwtProvider;
        private readonly IUserIdentity _userIdentity;
        
        public UserAuthenticationController(IBuildJwt jwtHelper)
        {
            _jwtProvider = jwtHelper;
        }

        [AllowAnonymous]//test 
        [HttpPost]
        public IActionResult Login([FromBody] User loginModel)
        {
            //XML xML = new XML(); 
            //xML.createAndLoadXML();
            // var a = xML.ReadXML("Connect", "ServerName");
            //xML.SerializeSettingToFile(xML.FILE_PATH);
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
