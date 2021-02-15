using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RESTStoreAPI.Setup.Config.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RESTStoreAPI.Services
{
    public interface ITokenService
    {
        TokenInfo GetToken(int id, string login, string name, List<string> roles);
    }
    public class TokenService : ITokenService
    {
        private readonly AuthConfigModel m_authConfig;
        public TokenService(IOptionsSnapshot<AuthConfigModel> authConfigModelAcc)
        {
            m_authConfig = authConfigModelAcc.Value;
        }
        public TokenInfo GetToken(int id, string login, string name, List<string> roles)
        {
            var handler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(m_authConfig.Key));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var claimsIdentity = GetClaimsIdentity(id, login, name, roles);
            var expires = DateTime.UtcNow.AddMinutes(m_authConfig.ExpiresMinutes);

            var token = handler.CreateJwtSecurityToken(subject: claimsIdentity,
                signingCredentials: signingCredentials,
                audience: m_authConfig.Audience,
                issuer: m_authConfig.Issuer,
                expires: expires);

            string tokenStr = handler.WriteToken(token);

            return new TokenInfo
            {
                Expires = expires,
                Token = tokenStr
            };
        }

        private static ClaimsIdentity GetClaimsIdentity(int id, string login, string name, List<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, login),
                new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                new Claim(ClaimTypes.GivenName, name)
            };

            foreach (var role in roles)
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
