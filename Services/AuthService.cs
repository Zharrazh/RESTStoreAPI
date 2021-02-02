﻿using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RESTStoreAPI.Config.Models;
using RESTStoreAPI.Data;
using RESTStoreAPI.Data.DbModels;
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
    }
    public class AuthService : IAuthService
    {
        private readonly AuthConfigModel authConfig;
        private readonly HttpContext ctx;
        private readonly DatabaseContext db;


        public AuthService(IConfiguration configuration, DatabaseContext db, IHttpContextAccessor ctxAcc)
        {
            authConfig = configuration.GetSection("Auth").Get<AuthConfigModel>();
            ctx = ctxAcc.HttpContext!;
            this.db = db;
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
                Roles = RoleUtils.GetRoleList(userDbModel.Roles),
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

        private static ClaimsIdentity GetClaimsIdentity (UserDbModel user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.GivenName, user.Name)
            };

            List<string> roleNames = RoleUtils.GetRoleList(user.Roles);

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
