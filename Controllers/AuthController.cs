using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTStoreAPI.Data;
using RESTStoreAPI.Data.DbModels;
using RESTStoreAPI.Models.Auth.GetToken;
using RESTStoreAPI.Models.Auth.Register;
using RESTStoreAPI.Models.Common;
using RESTStoreAPI.Models.User;
using RESTStoreAPI.Services;
using RESTStoreAPI.Utils;
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
        public AuthController(DatabaseContext db,  IAuthService authService, IPasswordService passwordService)
        {
            this.db = db;
            this.authService = authService;
            this.passwordService = passwordService;
        }

        [HttpPost("getToken")]
        [ProducesResponseType(typeof(GetTokenResponce), StatusCodes.Status200OK)]
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

        [HttpPost("registration")]
        [ProducesResponseType(typeof (UserFullInfo),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof (BadRequestType),StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if(await db.Users.AnyAsync(x=> x.Login == request.Login)){
                ModelState.AddModelError("login", "A user with this login already exists");
                return BadRequest(new BadRequestType(ModelState));
            }

            UserDbModel newUser = new UserDbModel
            {
                Name = request.Name,
                Login = request.Login,
                PasswordHash = passwordService.SaltHash(request.Password),
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                Roles = "u",
                IsActive = true
            };
            await db.Users.AddAsync(newUser);

            await db.SaveChangesAsync();
            return Ok(newUser.ToFullInfo());
        }
    }
}
