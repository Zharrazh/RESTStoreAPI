using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RESTStoreAPI.Config.Models;
using RESTStoreAPI.Data.DbModels;
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
    }
    public class AuthService : IAuthService
    {
        private readonly IRoleService roleService;
        private readonly AuthConfigModel authConfig;
        public AuthService(IRoleService roleService, IConfiguration configuration)
        {
            this.roleService = roleService;
            authConfig = configuration.GetSection("Auth").Get<AuthConfigModel>();
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
                Id = userDbModel.Id,
                Login = userDbModel.Login,
                Name = userDbModel.Name,
                Expires = expires,
                IsAdmin = userDbModel.Roles.Contains("a"),
                Roles = roleService.GetRoleList(userDbModel.Roles),
                Token = tokenStr
            };
        }

        private ClaimsIdentity GetClaimsIdentity (UserDbModel user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.GivenName, user.Name)
            };

            List<string> roleNames = roleService.GetRoleList(user.Roles);

            foreach (var role in roleNames)
                claims.Add(new Claim(ClaimTypes.Role, role));

            return new ClaimsIdentity(claims);
        }
    }

    public class TokenInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public List<string> Roles { get; set; }
        public bool IsAdmin { get; set; }
        public string Token { get; set; }
        public DateTime Expires { get; set; }
    }
}
