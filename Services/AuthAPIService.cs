using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RESTStoreAPI.Data;
using RESTStoreAPI.Data.DbModels;
using RESTStoreAPI.Models.Auth;
using RESTStoreAPI.Models.Auth.GetToken;
using RESTStoreAPI.Models.Auth.Register;
using RESTStoreAPI.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace RESTStoreAPI.Services
{
    public interface IAuthAPIService
    {
        Task<TokenInfoResponce> GetTokenAsync(GetTokenRequest request);
        Task<RegisterResponce> RegisterAsync(RegisterRequest request);
        Task<UserFullInfoResponce> MeAsync();
    }
    public class AuthAPIService : IAuthAPIService
    {
        private readonly DatabaseContext m_db;
        private readonly IAuthService m_authService;
        private readonly IPasswordService m_passwordService;
        private readonly IMapper m_mapper;
        private readonly ITokenService m_tokenService;
        private readonly IRoleService m_roleService;

        public AuthAPIService(DatabaseContext db, IAuthService authService, IPasswordService passwordService, IMapper mapper, ITokenService tokenService, IRoleService roleService)
        {
            m_db = db;
            m_authService = authService;
            m_passwordService = passwordService;
            m_mapper = mapper;
            m_tokenService = tokenService;
            m_roleService = roleService;
        }
        public async Task<TokenInfoResponce> GetTokenAsync(GetTokenRequest request)
        {
            var userDB = await m_db.Users.FirstOrDefaultAsync(x => x.Login == request.Login && x.PasswordHash == m_passwordService.SaltHash(request.Password));

            if (userDB is null)
                throw new WrongLoginOrPasswordException();

            if (!userDB.IsActive)
            {
                throw new UserNotActiveException();
            }

            var tokenInfo = m_tokenService.GetToken(userDB.Id, userDB.Login, userDB.Name, m_roleService.GetRoleNames(userDB.Roles));
            var tokenInfoResponce = m_mapper.Map<TokenInfoResponce>(tokenInfo);

            userDB.LastLoginDate = DateTime.UtcNow;
            await m_db.SaveChangesAsync();

            return tokenInfoResponce;
        }

        public async Task<UserFullInfoResponce> MeAsync()
        {
            if (m_authService.IsAuthUser())
                throw new AuthenticationException();
            var user = await m_authService.GetAuthUserAsync();

            if (user is null)
                throw new InvalidCredentialException();

            return m_mapper.Map<UserFullInfoResponce>(user);
        }

        public async Task<RegisterResponce> RegisterAsync(RegisterRequest request)
        {
            List<string> roles = new List<string> { Roles.UserRoleName };
            if (request.IsAdmin)
            {
                if (!m_authService.IsAuthUser())
                    throw new AuthenticationException();

                if (!m_authService.AuthUserInRole(Roles.AdminRoleName))
                    throw new UserNotAdminException();

                roles.Add(Roles.AdminRoleName);
            }

            var nowTime = DateTime.UtcNow;
            UserDbModel newUser = new UserDbModel
            {
                Name = request.Name,
                Login = request.Login,
                PasswordHash = m_passwordService.SaltHash(request.Password),
                Created = nowTime,
                Updated = nowTime,
                LastLoginDate = nowTime,
                Roles = m_roleService.GetRoleKeys(roles),
                IsActive = true
            };

            await m_db.Users.AddAsync(newUser);

            await m_db.SaveChangesAsync();

            var tokenInfo = m_tokenService.GetToken(newUser.Id, newUser.Login, newUser.Name, m_roleService.GetRoleNames(newUser.Roles));

            var tokenInfoResponce = m_mapper.Map<TokenInfoResponce>(tokenInfo);

            var userInfoResponce = m_mapper.Map<UserFullInfoResponce>(newUser);

            var registerResponce = new RegisterResponce
            {
                TokenInfo = tokenInfoResponce,
                UserInfo = userInfoResponce
            };

            return registerResponce;
        }
    }

    public class WrongLoginOrPasswordException : Exception { }
    public class UserNotActiveException : Exception { }
    public class UserNotAdminException : Exception { }
}
