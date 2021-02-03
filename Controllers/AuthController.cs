using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTStoreAPI.Data;
using RESTStoreAPI.Data.DbModels;
using RESTStoreAPI.Models.Auth;
using RESTStoreAPI.Models.Auth.GetToken;
using RESTStoreAPI.Models.Auth.Register;
using RESTStoreAPI.Models.Common;
using RESTStoreAPI.Models.User;
using RESTStoreAPI.Services;
using RESTStoreAPI.Utils;
using RESTStoreAPI.Utils.Constants;
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
        private readonly DatabaseContext db;
        private readonly IAuthService authService;
        private readonly IPasswordService passwordService;
        private readonly IMapper mapper;
        public AuthController(DatabaseContext db,  IAuthService authService, IPasswordService passwordService, IMapper mapper)
        {
            this.db = db;
            this.authService = authService;
            this.passwordService = passwordService;
            this.mapper = mapper;
        }

        [HttpPost("getToken")]
        [ProducesResponseType(typeof(TokenInfoResponce), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestType), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetToken(GetTokenRequest request)
        {
            var userDB = await db.Users.FirstOrDefaultAsync(x => x.Login == request.Login && x.PasswordHash== passwordService.SaltHash(request.Password));
            if (userDB == null)
            {
                ModelState.AddModelError("", "Wrong username or password");
                return BadRequest(new BadRequestType(ModelState));
            }
            if (!userDB.IsActive)
            {
                ModelState.AddModelError("", "You do not have permission to access the service");
                return BadRequest(new BadRequestType(ModelState));
            }

            var tokenInfo = authService.GetToken(userDB);

            var tokenInfoResponce = mapper.Map<TokenInfoResponce>(tokenInfo);
            return Ok(tokenInfoResponce);
        }

        [HttpPost("registration")]
        [ProducesResponseType(typeof (RegisterResponce),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof (BadRequestType),StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(RegisterRequest request, [FromServices] IAuthService authService)
        {
            if(await db.Users.AnyAsync(x=> x.Login == request.Login)){
                ModelState.AddModelError("login", "A user with this login already exists");
                return BadRequest(new BadRequestType(ModelState));
            }
            string roles = "u";
            if (request.IsAdmin)
            {
                if (HttpContext.User.IsInRole(RoleConstants.AdminRoleName))
                {
                    roles += "a";
                }
                else
                {
                    return Forbid();
                }
            }
            UserDbModel newUser = new UserDbModel
            {
                Name = request.Name,
                Login = request.Login,
                PasswordHash = passwordService.SaltHash(request.Password),
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                Roles = roles,
                IsActive = true
            };
            await db.Users.AddAsync(newUser);

            await db.SaveChangesAsync();

            var tokenInfo = authService.GetToken(newUser);

            var tokenInfoResponce = mapper.Map<TokenInfoResponce>(tokenInfo);

            var userInfoResponce = mapper.Map<UserFullInfoResponce>(newUser);

            var registerResponce = new RegisterResponce
            {
                TokenInfo = tokenInfoResponce,
                UserInfo = userInfoResponce
            };

            return Ok(registerResponce);
        }
    }
}
