using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RESTStoreAPI.Data;
using RESTStoreAPI.Data.DbModels;
using RESTStoreAPI.Setup.Config.Models;
using RESTStoreAPI.Utils;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RESTStoreAPI.Services
{
    public interface IAuthService
    {
        TokenInfo GetToken(UserDbModel userDbModel);
        public Task<UserDbModel> GetAuthUserAsync();

        public bool IsAuthUser(UserDbModel userDbModel);

        public bool AuthUserInRole(string roleName);
    }
    public class AuthService : IAuthService
    {
        private readonly AuthConfigModel authConfig;
        private readonly HttpContext ctx;
        private readonly DatabaseContext db;
        private readonly IRoleService roleService;


        public AuthService(IOptionsSnapshot<AuthConfigModel> authConfigModelAcc, DatabaseContext db, IHttpContextAccessor ctxAcc, IRoleService roleService)
        {
            this.authConfig = authConfigModelAcc.Value;
            ctx = ctxAcc.HttpContext!;
            this.db = db;
            this.roleService = roleService;
        }

        public TokenInfo GetToken(UserDbModel userDbModel)
        {
            var handler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authConfig.Key));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var claimsIdentity = GetClaimsIdentity(userDbModel);
            var expires = DateTime.UtcNow.AddMinutes(authConfig.Expires);

            var token = handler.CreateJwtSecurityToken(subject: claimsIdentity,
                signingCredentials: signingCredentials,
                audience: authConfig.Audience,
                issuer: authConfig.Issuer,
                expires: expires);

            string tokenStr = handler.WriteToken(token);

            return new TokenInfo
            {
                Expires = expires,
                Token = tokenStr
            };
        }

        public async Task<UserDbModel> GetAuthUserAsync()
        {
            if (ctx.User?.Identity?.Name == null)
                return null;
            else
            {
                return await db.Users.FirstOrDefaultAsync(u => u.Login == ctx.User.Identity.Name);
            }

        }

        public bool IsAuthUser(UserDbModel userDbModel)
        {

            if (ctx.User?.Identity?.Name == null)
                return false;
            else
            {
                return userDbModel.Login == ctx.User?.Identity?.Name;
            }
        }

        public bool AuthUserInRole(string roleName)
        {
            return ctx.User.IsInRole(roleName);
        }

        private ClaimsIdentity GetClaimsIdentity (UserDbModel user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.GivenName, user.Name)
            };

            List<string> roleNames = roleService.GetRoleNames(user.Roles);

            foreach (var role in roleNames)
                claims.Add(new Claim(ClaimTypes.Role, role));

            return new ClaimsIdentity(claims);
        }

        
    }

    public class TokenInfo
    {
        public string Token { get; set; }
        public DateTime Expires { get; set; }
    }
}
