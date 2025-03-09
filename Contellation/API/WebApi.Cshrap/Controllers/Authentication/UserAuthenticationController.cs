using Core.Api.Infrastractures.Repositories;
using Core.Api.JwtSetup;
using Core.Entities.Connects;
using Core.Entities.UserManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        /// <summary>
        /// {
        ///     "username": "AdminSSR",
        ///     "password": "123456"
        /// }
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns>Token</returns>

        [AllowAnonymous]//test 
        [HttpPost]
        public IActionResult Login([FromBody] object loginModel)
        {

            try
            {
                if (loginModel != null)
                {
                    _repository.UserIdentity.Identity(loginModel, _jwtProvider);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }
            //if (!loginModel.Username.IsNullOrEmpty() && !loginModel.Password.IsNullOrEmpty())
            //{
            //    string token = _jwtProvider.GenerateToken(_repository.UserIdentity.Identity(loginModel));
            //    if (!token.IsNullOrEmpty())
            //    {
            //        return Ok(new { token });
            //    }
            //    else
            //    {
            //        return NotFound();
            //    }
            //}

            return Unauthorized();
        }

    }
}
