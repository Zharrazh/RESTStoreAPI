using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RESTStoreAPI.Models.Auth.GetToken;
using RESTStoreAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IAuthService authService;
        public AuthController(IUserService userService, IAuthService authService)
        {
            this.userService = userService;
            this.authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> GetTokenAsync(GetTokenRequest request)
        {
            var userDB = await userService.GetUserAsync(request.Login, request.Password);
            if(userDB == null)
            {
                ModelState.AddModelError("", "Wrong username or password");
                return BadRequest(ModelState);
            }
            if (!userDB.IsActive)
            {
                ModelState.AddModelError("", "You do not have permission to access the service");
                return BadRequest(ModelState);
            }

            var tokenInfo = authService.GetToken(userDB);

            return Ok(new GetTokenResponce {
                Id = tokenInfo.Id,
                Login = tokenInfo.Login,
                Name = tokenInfo.Name,
                IsAdmin =tokenInfo.IsAdmin,
                Roles = tokenInfo.Roles,
                Expires = tokenInfo.Expires,
                Token = tokenInfo.Token
            });
        }
    }
}
